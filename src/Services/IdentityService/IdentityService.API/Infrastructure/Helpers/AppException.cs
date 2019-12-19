using System;
using System.Globalization;

namespace IdentityService.Infrastructure.Helpers
{
    public class AppException: Exception
    {
        public AppException(string message) : base(message) { }
    }
}