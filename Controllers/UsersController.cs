using Microsoft.AspNetCore.Mvc;
using CalenTaskApi.Respositories;
using CalenTaskApi.Dtos;
using CalenTaskApi.Entities;
using System.Text;
using System.Security.Cryptography;

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
            var hmac = new HMACSHA512();

            Users users = new(){
                Id = Guid.NewGuid(),
                FirstName = usersDto.FirstName,
                LastName = usersDto.LastName,
                UserName = usersDto.UserName,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(usersDto.Password)),
                PasswordSalt = hmac.Key,
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

        //User Login Route

        [HttpPost("login")]

        public async Task<ActionResult<Users>> LoginUserAsync(LoginDto loginDto)
        {
            var user = await repository.LoginUserAsync(loginDto.UserName);

            if(user is null)
            {
                return Unauthorized("Invalid Username");
            }

            var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for(int i = 0; i<computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                {
                 return Unauthorized("Invalid Password");
                }
            }

            return user;

        }
    }

}