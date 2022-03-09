using Microsoft.AspNetCore.Mvc;
using CalenTaskApi.Respositories;
using CalenTaskApi.Dtos;
using CalenTaskApi.Entities;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace CalenTaskApi.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository repository; //repository object goes here
        private readonly ITokenRepository tokenRepository;
        public UsersController(IUsersRepository repository, ITokenRepository tokenRepository) //dependency injection
        {
            this.repository = repository;
            this.tokenRepository = tokenRepository;
        }

        [EnableCors("AllowOriginsPolicy")]
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IEnumerable<UsersDto>> GetUsersAsync()
        {
            var users = (await repository.GetUsersAsync()).Select(users => users.AsDto());
            return users;
        } 

        [EnableCors("AllowOriginsPolicy")]
        [Authorize]
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

        [EnableCors("AllowOriginsPolicy")]
        [HttpPost]
        public async Task<ActionResult<UsersDto>> PostUsersAsync(PostUsersDto usersDto)
        {
            /* Create custom validator maybe
            if(usersDto.UserName)
            {
                return BadRequest("This Username is Already Taken");
            } */

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
                Role = "Basic",
                CreatedAt = DateTimeOffset.UtcNow,
                LastLogin = DateTimeOffset.UtcNow
            };

            await repository.PostUsersAsync(users);

            return CreatedAtAction(nameof(PostUsersAsync), new { id = users.Id }, users.AsDto()); 
        }

        [EnableCors("AllowOriginsPolicy")]
        [Authorize]
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

        [EnableCors("AllowOriginsPolicy")]
        [Authorize]
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
        
        [EnableCors("AllowOriginsPolicy")]
        [HttpPost("login")]

        public async Task<ActionResult> LoginUserAsync(LoginDto loginDto)
        {
            var user = await repository.LoginUserAsync(loginDto.UserName);

            if(user is null)
            {
                return Unauthorized("Invalid Username");
            }

            var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            
            if (computedHash.SequenceEqual(user.PasswordHash) == false)
            {
                return Unauthorized("Invalid Password");
            }
            
            string token = tokenRepository.CreateToken(user);
            user.Token = token;

            //return Ok("You are logged in.");
            return Ok(user);

        }
    }

}