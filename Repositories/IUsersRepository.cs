using CalenTaskApi.Entities;

namespace CalenTaskApi.Respositories
{
    public interface IUsersRepository
    {
        public IEnumerable<Users> GetUsers();
        public Users GetUser(Guid id);
        public Users PostUsers();
        public Users UpdateUser(Guid id);
        public Users DeleteUser(Guid id);
    }
}