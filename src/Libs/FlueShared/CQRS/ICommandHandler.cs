namespace FlueShared.CQRS
{
    public interface ICommandHandler<TCommand>
    {
        void Handle(TCommand cmd);
    }
}