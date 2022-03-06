using Microsoft.AspNetCore.Mvc;
using CalenTaskApi.Respositories;
using CalenTaskApi.Dtos;
using CalenTaskApi.Entities;

namespace CalenTaskApi.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository repository; //repository object goes here

        public UsersController(IUsersRepository repository) //dependency injection
        {
            this.repository = repository;
        }

    
        [HttpGet]
        public async Task<IEnumerable<UsersDto>> GetUsersAsync()
        {
            var users = (await repository.GetUsersAsync()).Select(users => users.AsDto());
            return users;
        } 


        [HttpGet("{id}")]
        public async Task<ActionResult<UsersDto>> GetUsersAsync(Guid id)
        {

            var users = await repository.GetUsersAsync(id);

            if(users is null)
            {
                return NotFound();
            }

            return users.AsDto();

        }

        [HttpPost]
        public async Task<ActionResult<UsersDto>> PostUsersAsync(PostUsersDto usersDto)
        {
            Users users = new(){
                Id = Guid.NewGuid(),
                FirstName = usersDto.FirstName,
                LastName = usersDto.LastName,
                UserName = usersDto.UserName,
                PassWord = usersDto.PassWord,
                Email = usersDto.Email,
                About = usersDto.About,
                CreatedAt = DateTimeOffset.UtcNow,
                LastLogin = DateTimeOffset.UtcNow
            };

            await repository.PostUsersAsync(users);

            return CreatedAtAction(nameof(PostUsersAsync), new { id = users.Id }, users.AsDto()); 
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUserAsync(Guid id, UpdateUsersDto updatedUserDto)
        {
            var existingUser = await repository.GetUsersAsync(id);

            if(existingUser is null)
            {
                return NotFound();
            }

            Users updatedUser = existingUser with {
                FirstName = updatedUserDto.FirstName,
                LastName = updatedUserDto.LastName,
                UserName = updatedUserDto.UserName,
                Email = updatedUserDto.Email,
                About = updatedUserDto.About,
                CreatedAt = DateTimeOffset.UtcNow,
                LastLogin = DateTimeOffset.UtcNow
            };

            await repository.UpdateUserAsync(updatedUser);

            return NoContent();
        }

        [HttpDelete("{id}")]

        public async Task<ActionResult> DeleteUserAsync(Guid id)
        {
            var existingUser = await repository.GetUsersAsync(id);

            if(existingUser is null)
            {
                return NotFound();
            }

            await repository.DeleteUserAsync(id);

            return NoContent();

        }
    }

}