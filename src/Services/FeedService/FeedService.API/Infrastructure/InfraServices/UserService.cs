using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace FeedService.Infrastructure.InfraServices
{
    public class UserService: IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId
        {
            get
            {
                Guid.TryParse(_httpContextAccessor.HttpContext.User.Claims
                    .Where(c => c.Type == ClaimTypes.NameIdentifier)
                    .Select(c => c.Value).SingleOrDefault(), out var userId);

                return userId;
            }
        }
    }

    public interface IUserService
    {
        Guid UserId { get; }
    }
}