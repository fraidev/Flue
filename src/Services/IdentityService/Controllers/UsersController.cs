using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using IdentityService.Domain.Commands;
using IdentityService.Domain.Entities;
using IdentityService.Domain.Services;
using IdentityService.Domain.State;
using IdentityService.Infrastructure.Broker;
using IdentityService.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace IdentityService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UsersController(
            IUserService userService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]UserCommand userCommand)
        {
            var user = _userService.Authenticate(userCommand.Username, userCommand.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });
            
            // return basic user info (without password) and token to store client side
            return Ok(new {
                Id = user.Id,
                Username = user.Username,
                /*FirstName = user.FirstName,
                LastName = user.LastName,*/
                Token = user.Token
            });
        }

/*        [HttpPost("Follow/{id}")]
        public IActionResult Follow(Guid id)
        {
            var claim = ((ClaimsIdentity)User.Identity);
            var userId = Guid.Parse(claim.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).SingleOrDefault());

            var user = _userService.GetById(userId);
            var follow = _userService.GetById(id);

            if (user.Following.Contains(follow))
            {
                throw new Exception("Esse usuario já esta sendo seguido");
            };

            user.Following.Add(follow);
            
            _userService.Update(user);
            return Ok();
        }*/

/*        [HttpPost("Unfollow/{id}")]
        public IActionResult Unfollow(Guid id)
        {
            var claim = ((ClaimsIdentity)User.Identity);
            var userId = Guid.Parse(claim.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).SingleOrDefault());

            var user = _userService.GetById(userId);
            var follow = _userService.GetById(id);

            if (!user.Following.Contains(follow))
            {
                throw new Exception("Esse não está sendo seguido");
            };

            user.Following.Remove(follow);
            
            _userService.Update(user);
            return Ok();
        }*/

        /*[HttpGet("Followers")]
        public IActionResult Followers()
        {
            var userId = Guid.Parse(((ClaimsIdentity)User.Identity).Claims
                .Where(c => c.Type == ClaimTypes.NameIdentifier)
                .Select(c => c.Value).SingleOrDefault());

            var user = _userService.GetById(userId);
            var followers = _userService.GetAll().Where(x => x.Following.Contains(user));
            var userDtos = _mapper.Map<IList<UserCommand>>(followers);
            return Ok(userDtos);
        }*/

/*        [HttpGet("Following")]
        public IActionResult Following()
        {
            var userId = Guid.Parse(((ClaimsIdentity)User.Identity).Claims
                .Where(c => c.Type == ClaimTypes.NameIdentifier)
                .Select(c => c.Value).SingleOrDefault());

            var user = _userService.GetById(userId);
            var userDtos = _mapper.Map<IList<UserCommand>>(user.Following);
            return Ok(userDtos);
        }*/

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]UserCommand userCommand)
        {
            // map dto to entity
            var user = _mapper.Map<UserState>(userCommand);
            user.Role = Role.User;

            try 
            {
                // save 
                _userService.Create(user, userCommand.Password);
                return Ok();
            } 
            catch(AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        public IActionResult MyEmail()
        {
            var claim = ((ClaimsIdentity)User.Identity);
            var email = claim.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).SingleOrDefault();
            return Ok(email);
        }
        
/*        [HttpGet]
        public IActionResult GetUsers(string searchText)
        {
            searchText = searchText.ToLower();
            var users =  _userService.GetAll().Where(x => x.FullName.Contains(searchText)
                || x.Username.ToLower().Contains(searchText));
            var userDtos = _mapper.Map<IList<UserCommand>>(users);
            return Ok(userDtos);
        }*/
        
        /*[HttpGet]
        [Authorize(Roles = Role.Admin)]
        public IActionResult GetAll()
        {
            var users =  _userService.GetAll();
            var userDtos = _mapper.Map<IList<UserCommand>>(users);
            return Ok(userDtos);
        }*/

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var user =  _userService.GetById(id);
            var userDto = _mapper.Map<UserCommand>(user);
            return Ok(userDto);
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody]UserCommand userCommand)
        {
            // map dto to entity and set id
            var user = _mapper.Map<UserState>(userCommand);
            user.Id = id;

            try 
            {
                // save 
                _userService.Update(user, userCommand.Password);
                return Ok();
            } 
            catch(AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _userService.Delete(id);
            return Ok();
        }
    }
}