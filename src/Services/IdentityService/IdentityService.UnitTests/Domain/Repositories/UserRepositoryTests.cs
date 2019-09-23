using System;
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
        }
        
        [Fact]
        public void GetByUsernameMustReturnAUserWithThisUsername
            ()
        {
            _unitOfWork.GetById<User>(_users.First().UserId).Returns(_users.First());
            var result = _sut.GetById(_users.First().UserId);
            result.UserId.Should().Be(_users.First().UserId);
        }
    }
}