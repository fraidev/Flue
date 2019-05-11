using System;
using System.Collections.Generic;
using System.Linq;
using FeedService.Domain.Read.Models;
using FeedService.Infrastructure.Persistence;

namespace FeedService.Domain.Read.Repositories
{
    public interface IUserReadRepository
    {
        IEnumerable<UserModel> GetAll();
        UserModel GetById(Guid id);
    }
    
    public class UserReadRepository: IUserReadRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserReadRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public IEnumerable<UserModel> GetAll()
        {
            return _unitOfWork.Query<UserModel>();
        }

        public UserModel GetById(Guid id)
        {
            return _unitOfWork.GetById<UserModel>(id);
        }
    }
}