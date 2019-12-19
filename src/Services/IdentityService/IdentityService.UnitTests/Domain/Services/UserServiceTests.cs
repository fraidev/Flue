using System.Linq;
using System.Security.Cryptography;
using System.Text;
using AutoFixture;
using FluentAssertions;
using IdentityService.Domain.Command;
using IdentityService.Domain.Repositories;
using IdentityService.Domain.Services;
using IdentityService.Domain.State;
using IdentityService.Infrastructure.Broker;
using IdentityService.Infrastructure.Helpers;
using Microsoft.Extensions.Options;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace IdentityService.UnitTests.Domain.Services
{
    public class UserServiceTests
    {
        private readonly IOptions<AppSettings> _appSettings;
        private readonly IMessageBroker _messageBroker;
        private readonly IUserRepository _userRepository;
        private readonly UserService _sut;
        private readonly Fixture _fixture;
        
        public UserServiceTests()
        {
            _fixture = new Fixture();
            _userRepository = Substitute.For<IUserRepository>();
            _messageBroker = Substitute.For<IMessageBroker>();
            _appSettings = Options.Create(new AppSettings());
            _appSettings.Value.Secret = _fixture.Create<string>();

            _sut = new UserService(_appSettings, _userRepository, _messageBroker);
        }

        [Fact]
        public void AuthenticateMustReturnAUser()
        {
            //Given
            var user = _fixture.Create<User>();
            var loginModel = _fixture.Create<LoginModel>();

            using (var hmac = new HMACSHA512())
            {
                user.PasswordSalt = hmac.Key;
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginModel.Password));
            }
            
            _userRepository.GetByUsername(loginModel.Username).Returns(user);

            //When
            var result = _sut.Authenticate(loginModel);

            //Then
            result.Should().NotBeNull();
            result.Token.Should().NotBeNullOrEmpty();
        }
       
        [Fact] 
        public void AuthenticateMustThrowExceptionWhenPasswordHashIsIncorrect()
        {
            //Given
            var user = _fixture.Create<User>();
            
            var loginModel = _fixture.Create<LoginModel>();
            
            using (var hmac = new HMACSHA512())
            {
                user.PasswordSalt = hmac.Key;
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginModel.Password));
            }

            user.PasswordHash[0] = _fixture.Create<byte>();
            
            _userRepository.GetByUsername(loginModel.Username).Returns(user);

            //When and Then
            Assert.Throws<AppException>(() => _sut.Authenticate(loginModel));
        }


        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("", null)]
        [InlineData(null, "")]
        public void AuthenticateMustThrowExceptionWhenUsernameOrPasswordIsNullOrWhiteSpace(string username, string password)
        {
            //Given
            var user = _fixture.Create<User>();
            var loginModel = new LoginModel()
            {
                Username = username,
                Password = password
            };

            //When and Then
            Assert.Throws<AppException>(() => _sut.Authenticate(loginModel));
        }
       
        [Fact] 
        public void AuthenticateMustThrowExceptionWhenUsernameIsIncorrect()
        {
            //Given
            var loginModel = _fixture.Create<LoginModel>();
            _userRepository.GetByUsername(loginModel.Username).ReturnsNull();

            //When and Then
            Assert.Throws<AppException>(() => _sut.Authenticate(loginModel));
        }
       
        [Fact] 
        public void AuthenticateMustThrowExceptionWhenPasswordHashIsWithIncorrectLength()
        {
            //Given
            var user = _fixture.Create<User>();
            
            var loginModel = _fixture.Create<LoginModel>();
            _userRepository.GetByUsername(loginModel.Username).Returns(user);
            
            using (var hmac = new HMACSHA512())
            {
                user.PasswordSalt = hmac.Key;
                user.PasswordHash = _fixture.CreateMany<byte>(63).ToArray();
            }

            //When and Then
            Assert.Throws<AppException>(() => _sut.Authenticate(loginModel));
        }
       
        [Fact] 
        public void AuthenticateMustThrowExceptionWhenPasswordSaltIsWithIncorrectLength()
        {
            //Given
            var user = _fixture.Create<User>();
            var loginModel = _fixture.Create<LoginModel>();
            _userRepository.GetByUsername(loginModel.Username).Returns(user);
            
            
            using (var hmac = new HMACSHA512())
            {
                user.PasswordSalt = _fixture.CreateMany<byte>(63).ToArray();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginModel.Password));
            }


            //When and Then
            Assert.Throws<AppException>(() => _sut.Authenticate(loginModel));
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
            //given
            var userCommand = _fixture.Create<UserModel>();

            //when
            _sut.Create(userCommand);
            
            //then
            _userRepository.Received(1).Save(Arg.Is<User>(x => x.Username == userCommand.Username));
            _messageBroker.Received(1).Call(Arg.Any<string>());
            _messageBroker.Received(1).Close();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("     ")]
        public void CreateMustThrowExceptionWhenPasswordIsNullOrWhiteSpace(string password)
        {
            //given
            var userCommand = _fixture.Create<UserModel>();
            userCommand.Password = password;
            

            //when
            Assert.Throws<AppException>(() => _sut.Create(userCommand));
        }

        [Fact]
        public void CreateMustThrowExceptionWhenUsernameAlreadyExist()
        {
            //given
            var userCommand = _fixture.Create<UserModel>();

            var user = _fixture.Create<User>();
            
            //when and then
            _userRepository.GetByUsername(userCommand.Username).Returns(user);
            Assert.Throws<AppException>(() => _sut.Create(userCommand));
        }

        [Fact]
        public void UpdateMustUpdateAExistentUser()
        {
            //given
            var user = _fixture.Create<User>();
            _userRepository.GetById(user.UserId).Returns(user);
            
            //when
            _sut.Update(user, _fixture.Create<string>());
            
            //then
            _userRepository.Received(1).Update(Arg.Any<User>());
        }
        
        [Fact]
        public void UpdateMustThrowExceptionWhenUserNotExits()
        {
            //given
            var user = _fixture.Create<User>();
            _userRepository.GetById(user.UserId).ReturnsNull();
            
            //when and then
            Assert.Throws<AppException>(() => _sut.Update(user, _fixture.Create<string>()));
        }
        
        [Fact]
        public void UpdateMustThrowExceptionWhenUsernameAlreadyExists()
        {
            //given
            var user = _fixture.Create<User>();
            var userParams = _fixture.Create<User>();
            userParams.UserId = user.UserId;
           
            _userRepository.GetById(user.UserId).Returns(user); 
            _userRepository.GetByUsername(userParams.Username).Returns(_fixture.Create<User>());

            //when and then
            Assert.Throws<AppException>(() => _sut.Update(userParams, _fixture.Create<string>()));
        }

        [Fact]
        public void DeleteMustDeleteAExistentUser()
        {
            //given
            var user = _fixture.Create<User>();
            _userRepository.GetById(user.UserId).Returns(user);
            
            //when
            _sut.Delete(user.UserId);
            
            //then
            _userRepository.Received(1).Delete(user);
        }
    }
}