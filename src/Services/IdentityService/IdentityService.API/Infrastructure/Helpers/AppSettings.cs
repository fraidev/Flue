using System.Diagnostics.CodeAnalysis;

namespace IdentityService.Infrastructure.Helpers
{
    [ExcludeFromCodeCoverage]
    public class AppSettings
    {
        public string Secret { get; set; }
        public string ConnectionString { get; set; }
        public string RabbitHost { get; set; }
    }
}