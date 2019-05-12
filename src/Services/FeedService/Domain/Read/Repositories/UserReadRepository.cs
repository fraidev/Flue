using System;
using System.Collections.Generic;
using System.Linq;
using FeedService.Domain.Read.Models;
using FeedService.Infrastructure.Persistence;

namespace FeedService.Domain.Read.Repositories
{
    public interface IPersonReadRepository
    {
        IEnumerable<PersonModel> GetAll();
        PersonModel GetById(Guid id);
    }
    
    public class PersonReadRepository: IPersonReadRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public PersonReadRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public IEnumerable<PersonModel> GetAll()
        {
            return _unitOfWork.Query<PersonModel>();
        }

        public PersonModel GetById(Guid id)
        {
            return _unitOfWork.GetById<PersonModel>(id);
        }
    }
}