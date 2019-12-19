using System.Threading.Tasks;
using FlueShared;
using MediatR;

namespace FeedService.Infrastructure.CQRS
{
    public interface IMediatorHandler
    {
        Task SendCommand<T>(T command) where T : Command;
    }

    public class InMemoryBus : IMediatorHandler
    {
        private readonly IMediator _mediator;

        public InMemoryBus(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task SendCommand<T>(T command) where T : Command
        {
            return _mediator.Send(command);
        }
    }
}