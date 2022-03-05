using Microsoft.AspNetCore.Mvc;
using CalenTaskApi.Entities;
using CalenTaskApi.Respositories;

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

        /*
        [HttpGet]

        public IEnumerable<Users> GetUsers()
        {
            return Enumerable;
        } */
    }

}