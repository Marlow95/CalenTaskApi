using System.Security.Claims;

namespace CalenTaskApi.Services
{
    public class ReadJwtTokenService : IReadJwtTokenService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        public ReadJwtTokenService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }
        
        public string GetUserId()
        {
            var result = string.Empty;

            if(httpContextAccessor.HttpContext != null)
            {
                result = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            return result;
        }

        public string GetUserName()
        {
            var result = string.Empty;

            if(httpContextAccessor.HttpContext != null)
            {
                result = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            }

            return result;
        }
    }
}