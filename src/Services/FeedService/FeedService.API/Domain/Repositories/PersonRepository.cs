using System;
using System.Collections.Generic;
using System.Linq;
using FeedService.Domain.Aggregates;
using FeedService.Domain.States;
using FeedService.Infrastructure.Persistence;

namespace FeedService.Domain.Repositories
{
    public interface IPersonRepository
    {
        //Write
        Person GetById(Guid id);
        PersonAggregate GetAggregateById(Guid id);
        PersonAggregate GetAggregateByUserId(Guid id);
        void Save(PersonAggregate personAggregate);
        void Delete(Guid id);
        void Update(PersonAggregate personAggregate);

        //Read
        IQueryable<Person> GetAll();
        Person GetByUserId(Guid id);
    }

    public class PersonRepository : IPersonRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public PersonRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Person GetById(Guid id)
        {
            return _unitOfWork.GetById<Person>(id);
        }

        public PersonAggregate GetAggregateById(Guid id)
        {
            return new PersonAggregate(_unitOfWork.GetById<Person>(id));
        }

        public PersonAggregate GetAggregateByUserId(Guid id)
        {
            return new PersonAggregate(GetByUserId(id));
        }

        public void Save(PersonAggregate personAggregate)
        {
            _unitOfWork.Save(personAggregate.GetState());
        }

        public void Delete(Guid id)
        {
            _unitOfWork.Delete(GetById(id));
        }

        public void Update(PersonAggregate personAggregate)
        {
            _unitOfWork.Update(personAggregate.GetState());
        }

        public IQueryable<Person> GetAll()
        {
            return _unitOfWork.Query<Person>();
        }

        public Person GetByUserId(Guid userId)
        {
            return _unitOfWork.Query<Person>().FirstOrDefault(x => x.UserId == userId);
        }
    }
}