using CalenTaskApi.Entities;

namespace CalenTaskApi.Respositories
{
    public interface ITodoRepository
    {
        public Task<IEnumerable<Todo>> GetTodoAsync();
        public Task<Todo> GetTodoAsync(Guid id);
        public Task PostTodoAsync(Todo todo);
        public Task UpdateTodoAsync(Todo todo);
        public Task DeleteTodoAsync(Guid id);
    }
}