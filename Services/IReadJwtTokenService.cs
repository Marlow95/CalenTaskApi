namespace CalenTaskApi.Services
{
    public interface IReadJwtTokenService
    {
        public string GetUserId();
        public string GetUserName();
    }
}