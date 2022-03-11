using CalenTaskApi.Entities;

namespace CalenTaskApi.Service
{
    public interface ITokenService
    {
        public string CreateToken(Users user);
        
    }
}