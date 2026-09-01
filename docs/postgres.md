# Implementação do banco PostegrSQL

O banco de dados relacional postgres é altamente utilizado junto à qualquer framework backend pela sua confiabilidade, por oferecer uma gama completa de recursos e pela baixa complexidade da sua implementação.

## Criando o banco de dados

Primeiro, é preciso criar o banco de dados, optei por um container postgres para comportar o banco. 

``` yml
services:
  postgres:
    image: postgres:16
    container_name: todo_db
    environment:
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: senhasegura
      POSTGRES_DB: todo_db
    ports:
      - "5432:5432"
    volumes:
      - pgdata:/var/lib/postgresql/data
volumes:
  pgdata:
```

## Conectando a API ao backend

Os pacotes utilizados foram `Microsoft.EntityFrameworkCore.Design` e `Npgsql.EntityFrameworkCore.PostgreSQL`

A classe `TodoCntext` foi reaproveitada do tutorial mencionado no README principal desse repositório, sendo assim, foi preciso alterar a string de conexão e o builder na classe `Program`.

String de conexão:

``` json
{
"ConnectionStrings": {
    "TodoContext": "Host=localhost;Port=5432;Database=todo_db;Username=postgres;Password=senhasegura"
  }
}
```

Builder:

``` c#
builder.Services.AddDbContext<TodoContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("TodoContext")));
```

## E por fim, a primeira migração

Para realizar esse processo, concedi privilégios de superuser para o user `postgres`. Como isso não é uma boa prática por não ser uma opção segura, criei um usuário com privilégios minimos para a API a partir desse ponto.

``` shell
dotnet tool install --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## Criando o usuário da API

### Porque criar um usuário para a API?

Um usuário para a API com o minimo de permissões que ela necessita reforça a segurança da aplicação, é uma excelente solução frente a oferecer privilégios root para o usuário utilizado pela API.

1. Criar o usuário da API:

Crie um usuário dedicado e com uma senha forte.

``` sql
CREATE USER api_user WITH PASSWORD 'SuaSenhaSuperSeguraAqui';
```

2. Revogar acessos públicos (Opcional, mas recomendado):

Por padrão, o PostgreSQL permite que qualquer usuário crie tabelas no esquema public. É mais seguro remover isso.

``` sql
REVOKE CREATE ON SCHEMA public FROM PUBLIC;
```

3. Conceder permissões apenas no banco correto:

Primeiro, permita que o usuário se conecte ao banco de dados da sua aplicação.

``` sql
GRANT CONNECT ON DATABASE seu_banco_de_dados TO api_user;
```

4. Liberar acesso às tabelas e sequências:

Conecte-se ao banco de dados da aplicação e dê as permissões de leitura e escrita. \
Para tabelas já existentes:

``` sql
GRANT USAGE ON SCHEMA public TO api_user;
GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA public TO api_user;
GRANT USAGE, SELECT ON ALL SEQUENCES IN SCHEMA public TO api_user;
```

**Para tabelas futuras (Automação):**

Para garantir que a API continue funcionando quando novas tabelas forem criadas (via Migrations do Entity Framework, por exemplo), defina os privilégios padrão:

``` sql
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT SELECT, INSERT, UPDATE, DELETE ON TABLES TO api_user;
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT USAGE, SELECT ON SEQUENCES TO api_user;
```

⚠️ O que muda no .NET / Entity Framework?

Se a sua API em .NET utiliza o Entity Framework Core (EF Core) para rodar as Migrations em produção automaticamente (context.Database.Migrate()), o usuário da API precisará de permissões administrativas de DDL (como `CREATE TABLE`, `ALTER TABLE`).

**Existem duas formas recomendadas de lidar com isso:**

Abordagem Ideal (Mais Segura): A API roda com o usuário restrito (api_user). As Migrations não são executadas pela API em produção. Em vez disso, você gera o script SQL no seu pipeline de CI/CD (dotnet ef migrations script) e o executa usando um usuário administrador temporário.

Abordagem Prática (Privilégio Intermediário): Se a API precisa rodar as migrations sozinha, conceda a permissão de `CREATE` no esquema apenas para ela. Nunca a transforme em `SUPERUSER`.

``` sql
GRANT CREATE ON SCHEMA public TO api_user;
```