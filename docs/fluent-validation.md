# Implementando FluentValidation em um CRUD de Tarefas com .NET

Quando desenvolvemos aplicações, garantir a qualidade dos dados é essencial. No caso de um CRUD de tarefas, não faz sentido permitir que sejam criadas tarefas sem título ou com datas inválidas. Para resolver isso de forma elegante, podemos usar o FluentValidation, uma biblioteca poderosa e flexível para validação de dados em .NET.

## O que é FluentValidation?

O FluentValidation é uma biblioteca que permite definir regras de validação de forma fluente e desacoplada da lógica de negócio. Isso torna o código mais limpo, reutilizável e fácil de manter.

## Estrutura do Projeto

Nosso exemplo é um CRUD simples de tarefas:

**Model:** Task \
**Controller:** TasksController \
**Service/Repository:** responsável pelas operações de persistência \
**Validator:** classe que define as regras de validação

## Criando o Model

``` csharp
public class Task
{
    public int Id { get; set; }
    public string Title { get; set; }
    public bool IsCompleted { get; set; }
}
```

## Implementando o Validator

``` csharp
using FluentValidation;

public class TaskValidator : AbstractValidator<Task>
{
    public TaskValidator()
    {
        RuleFor(t => t.Title)
            .NotEmpty().WithMessage("O título é obrigatório")
            .MaximumLength(100).WithMessage("O título deve ter no máximo 100 caracteres");

        RuleFor(t => t.DueDate)
            .GreaterThan(DateTime.Now).WithMessage("A data de vencimento deve ser futura");
    }
}
```

## Registrando no Program.cs

``` csharp
builder.Services.AddControllers();
builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<TaskValidator>());
```

## Usando no Controller

``` csharp
[HttpPost]
public IActionResult Create([FromBody] Task task)
{
    if (!ModelState.IsValid)
        return BadRequest(ModelState);

    // lógica para salvar a tarefa
    return Ok(task);
}
```

## Conclusão

Com o FluentValidation, conseguimos separar regras de validação da lógica de negócio, deixando o código mais organizado e fácil de evoluir. Além disso, a biblioteca oferece suporte a validações complexas, customizadas e até mesmo assíncronas.