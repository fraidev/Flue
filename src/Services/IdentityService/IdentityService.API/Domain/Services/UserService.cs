using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using FluentNHibernate.Conventions;
using FlueShared;
using FlueShared.Entities;
using IdentityService.Domain.Command;
using IdentityService.Domain.Repositories;
using IdentityService.Domain.State;
using IdentityService.Infrastructure.Broker;
using IdentityService.Infrastructure.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace IdentityService.Domain.Services
{
    public interface IUserService
    {
        User Authenticate(LoginModel loginModel);
        IEnumerable<User> GetAll();
        User GetById(Guid id);
        void Create(UserModel userModel);
        void Update(User user, string password = null);
        void Delete(Guid id);
    }

    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly IUserRepository _userRepository;
        private readonly IMessageBroker _messageBroker;

        public UserService(IOptions<AppSettings> appSettings, IUserRepository userRepository, 
            IMessageBroker messageBroker)
        {
            _appSettings = appSettings.Value;
            _userRepository = userRepository;
            _messageBroker = messageBroker;
        }

        public User Authenticate(LoginModel loginModel)
        {
            if (string.IsNullOrEmpty(loginModel.Username) || string.IsNullOrEmpty(loginModel.Password))
                throw new AppException("Username or password is incorrect");

            var user = _userRepository.GetByUsername(loginModel.Username);

            if (user == null)
                throw new AppException("Username incorrect");

            if (!VerifyPasswordHash(loginModel.Password, user.PasswordHash, user.PasswordSalt))
                throw new AppException("Password incorrect");
            
            user.Token = GenerateToken(user);
            return user;
        }

        public void Create(UserModel userModel)
        {
            var user = new User {Username = userModel.Username, Role = Role.User};

            if(_userRepository.GetByUsername(user.Username) != null)  
                throw new AppException("Username \"" + user.Username + "\" is already taken");

            CreatePasswordHash(userModel.Password, out var passwordHash, out var passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            
            _userRepository.Save(user);
            
            var cmd = new CreatePersonCommand()
            {
                IdentifierId = user.UserId,
                Username =  userModel.Username,
                Name = userModel.Name,
                Email = userModel.Email
            };
            
            var wrapper = new WrapperCommand(cmd);
            var cmdJson = JsonConvert.SerializeObject(wrapper);
            _messageBroker.Call(cmdJson);
            _messageBroker.Close();
        }

        public IEnumerable<User> GetAll()
        {
            return _userRepository.GetAll();
        }

        public User GetById(Guid id)
        {
            return _userRepository.GetById(id);
        }

        public void Update(User userParam, string password = null)
        {
            var user = _userRepository.GetById(userParam.UserId);

            if (user == null)
                throw new AppException("User not found");

            if (userParam.Username != user.Username && _userRepository.GetByUsername(userParam.Username) != null){
                throw new AppException("Username " + userParam.Username + " is already taken");
            }
            
            user.Username = userParam.Username;

            // update password if it was entered
            if (!string.IsNullOrWhiteSpace(password))
            {
                CreatePasswordHash(password, out var passwordHash, out var passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }
            
            _userRepository.Update(user);
        }

        public void Delete(Guid id)
        {
            var user = _userRepository.GetById(id);
            if (user != null)
            {
                _userRepository.Delete(user);
            }
        }

        private string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new []
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim(ClaimTypes.Name, user.Username)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), 
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new AppException("Password is required");
            }

            using var hmac = new System.Security.Cryptography.HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (storedHash?.Length != 64 || storedSalt?.Length != 128)
            {
                return false;
            }

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                if (computedHash.Where((t, i) => t != storedHash[i]).Any())
                {
                    return false;
                }
            }

            return true;
        }
    }
}