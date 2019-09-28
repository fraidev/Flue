using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
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
        User Authenticate(string username, string password);
        IEnumerable<User> GetAll();
        User GetById(Guid id);
        User Create(UserCommand identifyState, string password);
        void Update(User user, string password = null);
        void Delete(Guid id);
    }

    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly IUserRepository _userRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IMapper _mapper;

        public UserService(IOptions<AppSettings> appSettings, IUserRepository userRepository, 
            IMessageBroker messageBroker, IMapper mapper)
        {
            _appSettings = appSettings.Value;
            _userRepository = userRepository;
            _messageBroker = messageBroker;
            _mapper = mapper;
        }

        public User Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = _userRepository.GetByUsername(username);

            // check if username exists
            if (user == null)
                return null;

            // check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;
            
            // authentication successful so generate jwt token
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
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            // authentication successful
            return user;
        }

        public IEnumerable<User> GetAll()
        {
            return _userRepository.GetAll();
        }

        public User GetById(Guid id)
        {
            return _userRepository.GetById(id);
        }

        public User Create(UserCommand userCommand, string password)
        {
            // map dto to entity
            var identifierState = _mapper.Map<User>(userCommand);
            identifierState.Role = Role.User;
            
            // validation
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if(_userRepository.GetByUsername(identifierState.Username).IsAny())  
                throw new AppException("Username \"" + identifierState.Username + "\" is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            identifierState.PasswordHash = passwordHash;
            identifierState.PasswordSalt = passwordSalt;
            
            _userRepository.Save(identifierState);
            
            //Create a Person
            var cmd = new CreatePersonCommand()
            {
                IdentifierId = identifierState.UserId,
                Username =  userCommand.Username,
                Name = userCommand.Name,
                Email = userCommand.Email
            };
            
            var wrapper = new WrapperCommand(cmd);
            var cmdJson = JsonConvert.SerializeObject(wrapper);
            var response = _messageBroker.Call(cmdJson);
            _messageBroker.Close();

            return identifierState;
        }

        public void Update(User userParam, string password = null)
        {
            var user = _userRepository.GetById(userParam.UserId);

            if (user == null)
                throw new AppException("User not found");

            if (userParam.Username != user.Username)
            {
                if(_userRepository.GetByUsername(userParam.Username).IsAny())  
                    throw new AppException("Username " + userParam.Username + " is already taken");
            }
            
            user.Username = userParam.Username;

            // update password if it was entered
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

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
        
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}