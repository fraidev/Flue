using System;
using System.Collections.Generic;
using System.Linq;
using FeedService.Domain.Read.Models;
using FeedService.Infrastructure.Persistence;

namespace FeedService.Domain.Read.Repositories
{
    public interface IPostReadRepository
    {
        IQueryable<PostModel> GetFrosts();
        IQueryable<PostModel> GetFires();
        PostModel GetById(Guid id);
    }
    
    public class PostReadRepository: IPostReadRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public PostReadRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IQueryable<PostModel> GetFrosts()
        {
            return _unitOfWork.Query<PostModel>().Where(x => x.OnFire == false);
        }

        public IQueryable<PostModel> GetFires()
        {
            return _unitOfWork.Query<PostModel>().Where(x => x.OnFire);
        }

        public PostModel GetById(Guid id)
        {
            return _unitOfWork.GetById<PostModel>(id);
        }
    }
}