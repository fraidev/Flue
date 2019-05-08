using System;
using FeedService.Domain.Write.Aggregates;
using FeedService.Domain.Write.States;

namespace FeedService.Domain.Write.Repositories
{
    public interface IPersonRepository
    {
        PersonState GetById(Guid id);
        void Save(FeedAggregate feedAggregate);
        void Delete(Guid id);
        void Update(PersonAggregate feedAggregate);
    }
    public class PersonRepository: IPersonRepository
    {
        public PersonState GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Save(FeedAggregate feedAggregate)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Update(PersonAggregate feedAggregate)
        {
            throw new NotImplementedException();
        }
    }
}