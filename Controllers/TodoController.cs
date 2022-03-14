using CalenTaskApi.Dtos;
using CalenTaskApi.Entities;
using CalenTaskApi.Respositories;
using CalenTaskApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CalenTaskApi.Controllers
{
    [ApiController]
    [Route("todo")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoRepository repository;
        private readonly IReadJwtTokenService readJwt;
        public TodoController(ITodoRepository repository, IReadJwtTokenService readJwt)
        {
            this.repository = repository;
            this.readJwt = readJwt;
        }

        [EnableCors]
        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<TodoDto>> GetTodoAsync()
        {
            var todo = (await repository.GetTodoAsync()).Select(todo=> todo.AsDtoTodo());
            return todo;
        } 

        [EnableCors]
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

        [EnableCors]
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<TodoDto>> PostTodoAsync(PostTodoDto todoDto)
        {
            Todo todo = new(){
                Id = Guid.NewGuid(),
                Description= todoDto.Description,
                IsComplete = todoDto.IsComplete,
                CreatedAt = DateTimeOffset.UtcNow,
                UserId = readJwt.GetUserId()
            };

            await repository.PostTodoAsync(todo);

            return CreatedAtAction(nameof(PostTodoAsync), new { id = todo.Id }, todo.AsDtoTodo()); 
        }
        
        [EnableCors]
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTodoAsync(Guid id, UpdateTodoDto todoDto)
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
        
        [EnableCors]
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