using System.Linq;
using AutoFixture;
using FluentAssertions;
using IdentityService.Domain.Repositories;
using IdentityService.Domain.State;
using IdentityService.Infrastructure.Persistence;
using NSubstitute;
using Xunit;

namespace IdentityService.UnitTests.Domain.Repositories
{
    public class UserRepositoryTests
    {
        private readonly UserRepository _sut;
        private readonly Fixture _fixture;
        private readonly IQueryable<User> _users;
        private readonly IUnitOfWork _unitOfWork;

        public UserRepositoryTests()
        {
             _unitOfWork = Substitute.For<IUnitOfWork>();
            _fixture = new Fixture(); 
            _users = _fixture.CreateMany<User>(10).AsQueryable();

            _unitOfWork.Query<User>().Returns(_users);
            _sut = new UserRepository(_unitOfWork);
        }

        [Fact]
        public void GetByIdMustReturnAUserWithThisId()
        {
            _unitOfWork.GetById<User>(_users.First().UserId).Returns(_users.First());
            var result = _sut.GetById(_users.First().UserId);
            
            result.UserId.Should().Be(_users.First().UserId);
            _unitOfWork.Received(1).GetById<User>(_users.First().UserId);
        }
        
        [Fact]
        public void GetByUsernameMustReturnAUserWithThisUsername()
        {
            var result = _sut.GetByUsername(_users.First().Username);
            
            result.Username.Should().Be(_users.First().Username);
            _unitOfWork.Received(1).Query<User>();
        }
        
        [Fact]
        public void GetAllMustReturnAllUsers()
        {
            var result = _sut.GetAll();
            
            result.Should().Contain(_users);
            _unitOfWork.Received(1).Query<User>();
        }
        
        [Fact]
        public void SaveMustSaveAUsers()
        {
            var user = _fixture.Create<User>();
            _sut.Save(user);
            
            _unitOfWork.Received(1).Save(Arg.Is<User>(x => x.UserId == user.UserId));
        }
        
        [Fact]
        public void UpdateMustUpdateAUsers()
        {
            var oldUserName = _users.First().Username;

            var user = _users.First();
            user.Username = _fixture.Create<string>();
            
            _sut.Update(user);
            
            _unitOfWork.Received(1).Update(Arg.Is<User>
                (x => x.UserId == user.UserId
                      && x.Username == user.Username
                      && x.Username != oldUserName));
        }
        
        
        [Fact]
        public void DeleteMustDeleteAUsers()
        {
            var user = _users.First();
            _sut.Delete(user);
            
            _unitOfWork.Received(1).Delete(Arg.Is<User>
                (x => x.UserId == user.UserId));
        }
    }
}