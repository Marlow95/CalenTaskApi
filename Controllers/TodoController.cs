using CalenTaskApi.Dtos;
using CalenTaskApi.Entities;
using CalenTaskApi.Respositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CalenTaskApi.Controllers
{
    [ApiController]
    [Route("todo")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoRepository repository;
        public TodoController(ITodoRepository repository)
        {
            this.repository = repository;
        }

        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<TodoDto>> GetTodoAsync()
        {
            var todo = (await repository.GetTodoAsync()).Select(todo=> todo.AsDtoTodo());
            return todo;
        } 

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoDto>> GetTodoAsync(Guid id)
        {

            var todo = await repository.GetTodoAsync(id);

            if(todo is null)
            {
                return NotFound();
            }

            return todo.AsDtoTodo();

        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<TodoDto>> PostTodoAsync(TodoDto todoDto)
        {
            Todo todo = new(){
                Id = Guid.NewGuid(),
                Description= todoDto.Description,
                IsComplete = todoDto.IsComplete,
                CreatedAt = DateTimeOffset.UtcNow
            };

            await repository.PostTodoAsync(todo);

            return CreatedAtAction(nameof(PostTodoAsync), new { id = todo.Id }, todo.AsDtoTodo()); 
        }
        
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTodoAsync(Guid id, TodoDto todoDto)
        {
            var existingTodo = await repository.GetTodoAsync(id);

            if(existingTodo is null)
            {
                return NotFound();
            }

            Todo updatedTodo = existingTodo with {
                Description= todoDto.Description,
                IsComplete = todoDto.IsComplete,
                CreatedAt = DateTimeOffset.UtcNow
            };

            await repository.UpdateTodoAsync(updatedTodo);

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]

        public async Task<ActionResult> DeleteTodoAsync(Guid id)
        {
            var existingTodo = await repository.GetTodoAsync(id);

            if(existingTodo is null)
            {
                return NotFound();
            }

            await repository.DeleteTodoAsync(id);

            return NoContent();

        }
    }
    
}