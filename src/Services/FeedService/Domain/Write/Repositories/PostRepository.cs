using System;
using FeedService.Domain.Write.Aggregates;
using FeedService.Domain.Write.States;
using FeedService.Infrastructure.Persistence;

namespace FeedService.Domain.Write.Repositories
{
    public interface IFeedRepository
    {
        PostState GetById(Guid id);
        void Save(FeedAggregate spellbookAggregate);
        void Delete(Guid id);
        void Update(FeedAggregate spellbookAggregate);
    }

    public class FeedRepository: IFeedRepository
    {
        private IUnitOfWork _unitOfWork;

        public FeedRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public PostState GetById(Guid id)
        {
            return _unitOfWork.GetById<PostState>(id);
        }
    
        public void Save(FeedAggregate spellbookAggregate)
        {
            _unitOfWork.Save(spellbookAggregate.GetState());
            _unitOfWork.Flush();
        }

        public void Delete(Guid id)
        {
            _unitOfWork.Delete(GetById(id));
            _unitOfWork.Flush();
        }
        
        public void Update(FeedAggregate spellbookAggregate)
        {
            var state = spellbookAggregate.GetState();
            _unitOfWork.Update(state);
            _unitOfWork.Flush();
        }
    }
}