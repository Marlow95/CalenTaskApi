using Microsoft.AspNetCore.Mvc;
using CalenTaskApi.Respositories;
using CalenTaskApi.Dtos;
using CalenTaskApi.Entities;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using CalenTaskApi.Service;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authentication.Google;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Newtonsoft.Json;

namespace CalenTaskApi.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository repository; //repository object goes here
        private readonly ITokenService tokenService;
        public UsersController(IUsersRepository repository, ITokenService tokenService) //dependency injection
        {
            this.repository = repository;
            this.tokenService = tokenService;
        }

        [EnableCors]
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IEnumerable<UsersDto>> GetUsersAsync()
        {
            var users = (await repository.GetUsersAsync()).Select(users => users.AsDto());
            return users;
        } 

        [EnableCors]
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

        [EnableCors]
        [HttpPost]
        public async Task<ActionResult<UsersDto>> PostUsersAsync(PostUsersDto usersDto)
        {   
            /*
             //Searches for username this is also used in the login user route
            var user = await repository.GetLoginUserAsync(usersDto.UserName);

            //Checks if Username Already Exists
            if(user.UserName.Equals(usersDto.UserName))
            {
                return BadRequest("This Username is Already Taken");
            };*/

            var hmac = new HMACSHA512();

            Users users = new(){
                Id = Guid.NewGuid(),
                FirstName = usersDto.FirstName,
                LastName = usersDto.LastName,
                UserName = usersDto.UserName,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(usersDto.PassWord)),
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

        [EnableCors]
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

        [EnableCors]
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
        
        [EnableCors]
        [HttpPost("login")]

        public async Task<ActionResult> GetLoginUserAsync(LoginDto loginDto)
        {
            //Searches for the Users Username
            var user = await repository.GetLoginUserAsync(loginDto.UserName);

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
            
            string token = tokenService.CreateToken(user);
            user.Token = token;
            user.LastLogin = DateTimeOffset.UtcNow;
            user.IsSuccess = true;

            //Response.Cookies.Append("X-Access-Token", token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });

            //return Ok("You are logged in.");
            return Ok(user);

        }


        /*
        [HttpPost("google/login")]
        public IActionResult LoginWithGoogle(GoogleLoginDto googleLoginDto)
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);

        }

        [HttpPost("google/response")]
        public async Task<ActionResult<string>> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    
            var claims = result.Principal.Identities
                .FirstOrDefault().Claims.Select(claim => new
            {
                claim.Issuer,
                claim.OriginalIssuer,
                claim.Type,
                claim.Value
            });

            var claimsJson = JsonConvert.SerializeObject(claims);

            return claimsJson;
        } 
        */
    }

}