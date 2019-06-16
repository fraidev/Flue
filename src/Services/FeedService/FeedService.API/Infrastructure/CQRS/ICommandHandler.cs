namespace FeedService.Infrastructure.CQRS
{
    public interface ICommandHandler<TCommand>
    {
        void Handle(TCommand cmd);
    }
}