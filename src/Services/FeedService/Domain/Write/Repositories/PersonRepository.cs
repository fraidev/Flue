using System;
using FeedService.Domain.Write.Aggregates;
using FeedService.Domain.Write.States;
using FeedService.Infrastructure.Persistence;

namespace FeedService.Domain.Write.Repositories
{
    public interface IPersonRepository
    {
        PersonState GetById(Guid id);
        void Save(PersonAggregate personAggregate);
        void Delete(Guid id);
        void Update(PersonAggregate personAggregate);
    }
    public class PersonRepository: IPersonRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public PersonRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public PersonState GetById(Guid id)
        {
            return _unitOfWork.GetById<PersonState>(id);
        }

        public void Save(PersonAggregate personAggregate)
        {
            _unitOfWork.Save(personAggregate.GetState());
//            _unitOfWork.Flush();
        }

        public void Delete(Guid id)
        {
            _unitOfWork.Delete(GetById(id));
//            _unitOfWork.Flush();
        }

        public void Update(PersonAggregate personAggregate)
        {
            _unitOfWork.Update(personAggregate.GetState());
//            _unitOfWork.Flush();
        }
    }
}