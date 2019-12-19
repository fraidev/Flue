using AutoFixture;
using FeedService.Infrastructure.CQRS;
using FlueShared;
using MediatR;
using NSubstitute;
using Xunit;

namespace FeedService.UnitTests.Infrastructure.CQRS
{
    public class InMemoryBusTests
    {
        private IMediator _mediator;
        private InMemoryBus _sut;
        private Fixture _fixture;

        public InMemoryBusTests()
        {
            _fixture = new Fixture();
            _mediator = Substitute.For<IMediator>();
            _sut = new InMemoryBus(_mediator);
        }

        [Fact]
        public void SendCommandMustSendACommandInMediator()
        {
            var cmd = _fixture.Create<Command>();

            _sut.SendCommand(cmd);

            _mediator.Received(1).Send(cmd);
        }
    }
}