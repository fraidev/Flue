using System;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using FlueShared.Entities;
using IdentityService.Domain.Models;
using IdentityService.Domain.Services;
using IdentityService.Domain.State;
using IdentityService.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace IdentityService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class IdentifyController : ControllerBase
    {
        private IUserService _userService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public IdentifyController(
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
        public IActionResult Authenticate([FromBody]Identifier identifier)
        {
            var user = _userService.Authenticate(identifier.Username, identifier.Password);

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

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]Identifier identifier)
        {
            // map dto to entity
            var user = _mapper.Map<UserState>(identifier);
            user.Role = Role.User;

            try 
            {
                // save 
                _userService.Create(user, identifier.Password);
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
        
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var user =  _userService.GetById(id);
            var userDto = _mapper.Map<Identifier>(user);
            return Ok(userDto);
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody]Identifier identifier)
        {
            // map dto to entity and set id
            var user = _mapper.Map<UserState>(identifier);
            user.Id = id;

            try 
            {
                // save 
                _userService.Update(user, identifier.Password);
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