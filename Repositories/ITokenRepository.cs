using CalenTaskApi.Entities;

namespace CalenTaskApi.Respositories
{
    public interface ITokenRepository
    {
        public string CreateToken(Users user);
        
    }
}