using FeedService.Infrastructure.Extensions;
using Xunit;

namespace FeedService.UnitTests.Infrastructure.Extensions
{
    public class StringExtensionsTest
    {
        [Theory]
        [InlineData( "FELIPE CaRlos RiBeiRO CarDOZO")]
        [InlineData("felipe carlos ribeiro cardozo")]
        [InlineData("FELIPE CARLOS RIBEIRO CARDOZO")]
        public void CapitalizeName(string name)
        {
            
            var result = name.Capitalize();
            
            Assert.Equal("Felipe Carlos Ribeiro Cardozo", result);
        }
    }
}