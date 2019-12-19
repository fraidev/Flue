using System.Linq;
using AutoFixture;

namespace FeedService.UnitTests
{
    public class BaseUnitTest
    {
        public IFixture Fixture { get; set; }

        public BaseUnitTest()
        {
            Fixture = new Fixture();
            Fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => Fixture.Behaviors.Remove(b));
            Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }
    }
}