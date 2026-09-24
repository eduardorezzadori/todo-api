using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.DTOs;
using TodoApi.Models;
using TodoApi.Resources;
using TodoApi.Services;
using TodoApi.Validators;

[Route("api/[controller]")]
[ApiController]
public class TodoItemsController : ControllerBase
{
    private readonly TodoService _service;
    public TodoItemsController(TodoService service)
    {
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
    public async Task<ActionResult<TodoItemDTO>> GetTodoItem(Guid id)
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
    public async Task<IActionResult> PutTodoItem(Guid? id, UpdateTodoItemDTO todoitem)
    {
        try
        {
            await _service.UpdateAsync(id, todoitem);
            return NoContent();

        }
        catch (DbUpdateConcurrencyException)
        {
            throw;
        }

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
    public async Task<IActionResult> DeleteTodoItem(Guid? id)
    {
        var deleteResult = await _service.DeleteAsync(id);

        if (deleteResult == false)
        {
            NotFound(new { message = ResourceMessages.TODO_NOT_FOUND });
        }

        return NoContent();
    }

}
