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
                CreatedAt = users.CreatedAt,
                LastLogin = users.LastLogin
            };
        }

        public static TodoDto AsDtoTodo(this Todo todo)
        {
            return new TodoDto {
                Id = todo.Id,
                Description = todo.Description,
                IsComplete = todo.IsComplete,
                UserId = todo.UserId,
                CreatedAt = todo.CreatedAt
            };
        }
    }
}