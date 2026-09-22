using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using TodoApi.Resources;
using TodoApi.Services;
using TodoApi.Validators;

[Route("api/[controller]")]
[ApiController]
public class TodoItemsController : ControllerBase
{
    private readonly TodoContext _context;
    private readonly TodoService _service;
    public TodoItemsController(TodoContext context, TodoService service)
    {
        _context = context;
        _service = service;
    }

    // GET: api/TodoItem
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItem()
    {
        var todoItems = await _service.GetAllTodoItemsAsync();
        return Ok(todoItems);
    }

    // GET: api/TodoItem/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
    {
        var todoitem = await _service.GetTodoItemAsync(id);

        if (todoitem == null)
        {
            return NotFound(new { message = ResourceMessages.TODO_NOT_FOUND });
        }

        return todoitem;
    }

    // PUT: api/TodoItem/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTodoItem(long? id, UpdateTodoItemDTO todoitem)
    {
        var existing = await _context.TodoItems.FindAsync(id);
        if (existing == null)
        {
            return NotFound();
        }

        if (!todoitem.IsComplete.HasValue)
        {
            return BadRequest(new { message = ResourceMessages.ISCOMPLETE_REQUIRED });
        }

        // apply updates from the incoming DTO to the existing TodoItemDTO
        todoitem.Adapt(existing);

        UpdateTodoItemUseCase validator = new UpdateTodoItemUseCase();

        // validate the TodoItemDTO instance expected by the validator
        var validatorResult = validator.Validate(existing);
        if (!validatorResult.IsValid)
        {
            return BadRequest(validatorResult.Errors);
        }

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            throw;
        }

        return NoContent();
    }

    // POST: api/TodoItem
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<TodoItemDTO>> PostTodoItem(TodoItemDTO todoitem)
    {
        CreateTodoItemUseCase validator = new CreateTodoItemUseCase();

        var validatorResult = validator.Validate(todoitem);

        if (!validatorResult.IsValid)
        {
            return BadRequest(validatorResult.Errors);
        }

        var createdTodoItem = await _service.CreateAsync(todoitem);

        return CreatedAtAction(nameof(GetTodoItem), new { id = createdTodoItem.Id }, createdTodoItem);
    }

    // DELETE: api/TodoItem/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoItem(long? id)
    {
        var deleteResult = await _service.DeleteAsync(id);

        if (deleteResult == false)
        {
            NotFound(new { message = ResourceMessages.TODO_NOT_FOUND });
        }

        return NoContent();
    }

}
