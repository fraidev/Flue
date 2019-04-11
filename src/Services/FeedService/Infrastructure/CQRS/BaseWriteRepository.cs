using FeedService.Infrastructure.Persistence;

namespace FeedService.Infrastructure.CQRS
{
    public interface IBaseWriteRepository <TState, TAggregate>
    {
        void Save(params TAggregate[] aggregates);
    }
    
    public class BaseWriteRepository<TState, TAggregete>: IBaseWriteRepository<TState, TAggregete> 
        where TAggregete: IBaseAggregate<TState>
        where TState : class
    {
        private readonly IUnitOfWork _unitOfWork;

        public BaseWriteRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public void Save(params TAggregete[] aggregates)
        {
            foreach (var aggregate in aggregates)
            {
                var state = aggregate.GetState();
                if (!_unitOfWork.Contains(state))
                {
                    _unitOfWork.Save(state);
                }
                _unitOfWork.Flush();
            }
        }
    }
}