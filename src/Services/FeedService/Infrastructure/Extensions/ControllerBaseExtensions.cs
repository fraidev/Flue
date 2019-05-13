using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace FeedService.Infrastructure.Extensions
{
    public static class ControllerBaseExtensions
    {
        public static Guid GetUserId(this ControllerBase controllerBase)
        {
            return Guid.Parse(((ClaimsIdentity)controllerBase.User.Identity)
                .Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).SingleOrDefault());
        }
    }
}