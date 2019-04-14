using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AccountService.Domain.Write.Commands;
using AccountService.Domain.Write.Entities;
using AccountService.Domain.Write.Services;
using AccountService.Domain.Write.State;
using AccountService.Infrastructure.Helpers;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AccountService.Controllers
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
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = user.Token
            });
        }

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
        
        [HttpGet]
        [Authorize(Roles = Role.Admin)]
        public IActionResult GetAll()
        {
            var users =  _userService.GetAll();
            var userDtos = _mapper.Map<IList<UserCommand>>(users);
            return Ok(userDtos);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user =  _userService.GetById(id);
            var userDto = _mapper.Map<UserCommand>(user);
            return Ok(userDto);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody]UserCommand userCommand)
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
        public IActionResult Delete(int id)
        {
            _userService.Delete(id);
            return Ok();
        }
    }
}