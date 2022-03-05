using CalenTaskApi.Entities;
using CalenTaskApi.Dtos;

namespace CalenTaskApi 
{
    public static class Extentions
    {
        public static UsersDto AsDto(this Users users)
        {
            return new UsersDto {
                Id = users.Id,
                FirstName = users.FirstName,
                LastName = users.LastName,
                UserName = users.UserName,
                Email = users.Email,
                About = users.About,
                Status = users.Status,
                Role = users.Role,
                CreatedAt = users.CreatedAt,
                LastLogin = users.LastLogin
            };
        }

        public static TodoDto AsDto(this Todo todo)
        {
            return new TodoDto {
                TodoId = todo.TodoId,
                Description = todo.Description,
                IsComplete = todo.IsComplete
            };
        }
    }
}