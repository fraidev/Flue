using System.Security.Cryptography;
using System.Text;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using IdentityService.Domain.Repositories;
using IdentityService.Domain.Services;
using IdentityService.Domain.State;
using IdentityService.Infrastructure.Broker;
using IdentityService.Infrastructure.Helpers;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace IdentityService.UnitTests.Domain.Services
{
    public class UserServiceTests
    {
        public UserServiceTests()
        {
            _fixture = new Fixture();
            _userRepository = Substitute.For<IUserRepository>();
            _messageBroker = Substitute.For<IMessageBroker>();
            _mapper = Substitute.For<IMapper>();
            _appSettings = Options.Create(new AppSettings());
            _appSettings.Value.Secret = _fixture.Create<string>();

            _sut = new UserService(_appSettings, _userRepository, _messageBroker, _mapper);
        }

        private readonly IOptions<AppSettings> _appSettings;
        private readonly IMapper _mapper;
        private readonly IMessageBroker _messageBroker;
        private readonly IUserRepository _userRepository;
        private readonly UserService _sut;
        private readonly Fixture _fixture;

        [Fact]
        public void AuthenticateMustReturnAUser()
        {
            //Given
            var user = _fixture.Create<User>();
            var password = _fixture.Create<string>();
            _userRepository.GetByUsername(user.Username).Returns(user);

            using (var hmac = new HMACSHA512())
            {
                user.PasswordSalt = hmac.Key;
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }

            //When
            var result = _sut.Authenticate(user.Username, password);

            //Then
            result.Should().NotBeNull();
            result.Token.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void GetAllMustReturnAllUsers()
        {
            var users = _fixture.CreateMany<User>(3);
            _userRepository.GetAll().Returns(users);

            var result = _sut.GetAll();

            _userRepository.Received(1).GetAll();
            result.Should().NotBeNullOrEmpty().And.HaveCount(3);
        }

        [Fact]
        public void GetByIdMustReturnUserWithTheSameId()
        {
            var user = _fixture.Create<User>();
            _userRepository.GetById(user.UserId).Returns(user);

            var result = _sut.GetById(user.UserId);

            _userRepository.Received(1).GetById(user.UserId);
            result.Should().Be(user);
        }

        [Fact]
        public void CreateMustCreateANewUser()
        {
            
        }

        [Fact]
        public void UpdateMustUpdateAExistentUser()
        {
            
        }

        [Fact]
        public void DeleteMustDeleteAExistentUser()
        {
            
        }
    }
}