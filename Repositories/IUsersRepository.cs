using CalenTaskApi.Entities;

namespace CalenTaskApi.Respositories
{
    public interface IUsersRepository
    {
        public Task<IEnumerable<Users>> GetUsersAsync();
        public Task<Users> GetUsersAsync(Guid id);
        public Task PostUsersAsync(Users users);
        public Task UpdateUserAsync(Users user);
        public Task DeleteUserAsync(Guid id);
    }
}