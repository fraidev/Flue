using System;
using System.Collections.Generic;
using System.Linq;
using FeedService.Domain.Read.Models;
using FeedService.Infrastructure.Persistence;

namespace FeedService.Domain.Read.Repositories
{
    public interface IPostReadRepository
    {
        IQueryable<PostModel> GetAll();
        PostModel GetById(Guid id);
        IEnumerable<PostModel> GetMyFeed(PersonModel personModel);
    }
    
    public class PostReadRepository: IPostReadRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public PostReadRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IQueryable<PostModel> GetAll()
        {
            return _unitOfWork.Query<PostModel>();
        }
        public IEnumerable<PostModel> GetMyFeed(PersonModel personModel)
        {
            var followersPersonId = personModel.Following.Select(x => x.PersonId);
            return _unitOfWork.Query<PostModel>().Where(x => followersPersonId.Contains(x.PersonId));
        }

        public PostModel GetById(Guid id)
        {
            return _unitOfWork.GetById<PostModel>(id);
        }
    }
}