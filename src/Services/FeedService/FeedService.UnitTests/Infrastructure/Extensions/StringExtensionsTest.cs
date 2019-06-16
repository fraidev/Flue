using FeedService.Infrastructure.Extensions;
using Xunit;

namespace FeedService.UnitTests.Infrastructure.Extensions
{
    public class StringExtensionsTest
    {
        [Fact]
        public void CapitalizeName()
        {
            const string name = "BetHany AVERY seCOND";
            
            var result = name.Capitalize();
            
            Assert.Equal("Bethany Avery Second", result);
        }
    }
}