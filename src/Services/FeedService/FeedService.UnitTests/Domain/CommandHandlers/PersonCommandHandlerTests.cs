using System;
using System.Linq;
using System.Threading;
using AutoFixture;
using FeedService.Domain.Aggregates;
using FeedService.Domain.CommandHandlers;
using FeedService.Domain.Commands.PersonCommands;
using FeedService.Domain.Repositories;
using FeedService.Domain.States;
using FlueShared;
using NSubstitute;
using Xunit;

namespace FeedService.UnitTests.Domain.CommandHandlers
{
    public class PersonCommandHandlerTests
    {
        private readonly IPersonRepository _personRepository;
        private PersonCommandHandler _sut;
        private Fixture _fixture;

        public PersonCommandHandlerTests()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            
            _personRepository = Substitute.For<IPersonRepository>();
            _sut = new PersonCommandHandler(_personRepository);
        }
        [Fact]
        public void CreatePersonHandleMustUseRepositorySaveForSaveAggregate()
        {
            //Given
            var cmd = _fixture.Create<CreatePersonCommand>();


            //When
            _sut.Handle(cmd, CancellationToken.None);
            
            //Then
            _personRepository.Received().Save(
                Arg.Is<PersonAggregate>(
                    x => x.GetState().Name == cmd.Name
                         && x.GetState().Email == cmd.Email
                         && x.GetState().UserId == cmd.IdentifierId
                         && x.GetState().Username == cmd.Username));
        }

        [Fact]
        public void UpdatePersonCommandMustUseRepositoryForUpdateAggregate()
        {
            //Given
            var cmd = _fixture.Create<UpdatePersonCommand>();
            var person = _fixture.Create<Person>();
            person.PersonId = cmd.PersonId;
            
            _personRepository.GetById(cmd.PersonId).Returns(person);
 
            //When
            _sut.Handle(cmd, CancellationToken.None);

            //Then
            _personRepository.Received(1).Update(
                Arg.Is<PersonAggregate>(x => x.GetState().Name == cmd.Name
                         && x.GetState().Email == cmd.Email
                         && x.GetState().PersonId == cmd.PersonId
                         && x.GetState().Description == cmd.Description
                         && x.GetState().ProfilePicture == cmd.ProfilePicture));
        }

        [Fact]
        public void FollowPersonCommandMustToFollowAPersonAndUpdateWithRepository()
        {
            //Given
            var person = _fixture.Create<Person>();
            var follow = _fixture.Create<Person>();
            
            var cmd = new FollowPersonCommand
            {
                PersonId  = person.PersonId,
                FollowId = follow.PersonId
            };
            
            _personRepository.GetById(person.PersonId).Returns(person);
            _personRepository.GetById(follow.PersonId).Returns(follow);
            
            //When
            _sut.Handle(cmd, CancellationToken.None);

           
            //Then 
            _personRepository.Received(1).Update(
                Arg.Is<PersonAggregate>(x => x.GetState().Following.Contains(follow)));
        }
        
        [Fact]
        public void FollowPersonCommandMustThrowExceptionWhenPersonAlreadyFollowThePersonInCommand()
        {
            var person = _fixture.Create<Person>();
            var follow = _fixture.Create<Person>();
            
            var cmd = new FollowPersonCommand
            {
              PersonId  = person.PersonId,
              FollowId = follow.PersonId
            };
            
            person.Following.Add(follow);

            _personRepository.GetById(person.PersonId).Returns(person);
            _personRepository.GetById(follow.PersonId).Returns(follow);

            Assert.ThrowsAsync<Exception>(() => _sut.Handle(cmd, CancellationToken.None));
        }
        
        

        [Fact]
        public void UnFollowPersonCommandMustToUnFollowAPersonAndUpdateWithRepository()
        {
            //Given
            var person = _fixture.Create<Person>();
            var unfollow = _fixture.Create<Person>();
            
            var cmd = new UnfollowPersonCommand
            {
                PersonId  = person.PersonId,
                UnfollowId = unfollow.PersonId
            };
            
            person.Following.Add(unfollow);
            
            _personRepository.GetById(person.PersonId).Returns(person);
            _personRepository.GetById(unfollow.PersonId).Returns(unfollow);
            
            //When
            _sut.Handle(cmd, CancellationToken.None);

           
            //Then 
            _personRepository.Received(1).Update(
                Arg.Is<PersonAggregate>(x => !x.GetState().Following.Contains(unfollow)));
        }
        
        [Fact]
        public void UnFollowPersonCommandMustThrowExceptionWhenPersonIsNotFollowedByThePersonInCommand()
        {
            var person = _fixture.Create<Person>();
            var unfollow = _fixture.Create<Person>();
            
            var cmd = new UnfollowPersonCommand
            {
                PersonId  = person.PersonId,
                UnfollowId = unfollow.PersonId
            };

            _personRepository.GetById(person.PersonId).Returns(person);
            _personRepository.GetById(unfollow.PersonId).Returns(unfollow);

            Assert.ThrowsAsync<Exception>(() => _sut.Handle(cmd, CancellationToken.None));
        }
    }
}