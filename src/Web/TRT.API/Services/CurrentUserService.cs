using System.Security.Claims;
using TRT.Application.Common.Interfaces;

namespace TRT.API.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }
        public string? UserId
        {
            get
            {
                if (_httpContextAccessor.HttpContext == null)
                    return (string?)null;

                try
                {
                    return (_httpContextAccessor.HttpContext.User?
                            .FindFirstValue(ClaimTypes.NameIdentifier))
                            .ToString();
                }
                catch (Exception ex)
                {
                    return (string?)null;
                }


            }
        }
    }
}
