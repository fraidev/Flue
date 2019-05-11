using System;
using FeedService.Domain.Write.Aggregates;
using FeedService.Domain.Write.States;
using FeedService.Infrastructure.Persistence;

namespace FeedService.Domain.Write.Repositories
{
    public interface IUserRepository
    {
        UserState GetById(Guid id);
        void Save(UserAggregate userAggregate);
        void Delete(Guid id);
        void Update(UserAggregate userAggregate);
    }
    public class UserRepository: IUserRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public UserState GetById(Guid id)
        {
            return _unitOfWork.GetById<UserState>(id);
        }

        public void Save(UserAggregate userAggregate)
        {
            _unitOfWork.Save(userAggregate.GetState());
//            _unitOfWork.Flush();
        }

        public void Delete(Guid id)
        {
            _unitOfWork.Delete(GetById(id));
//            _unitOfWork.Flush();
        }

        public void Update(UserAggregate userAggregate)
        {
            _unitOfWork.Update(userAggregate.GetState());
//            _unitOfWork.Flush();
        }
    }
}