using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using IdentityService.Domain.Command;
using IdentityService.Domain.Services;
using IdentityService.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers
{
    [ExcludeFromCodeCoverage]
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class IdentifyController : ControllerBase
    {
        private readonly IUserService _userService;

        public IdentifyController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]LoginModel loginModel)
        {
            var user = _userService.Authenticate(loginModel);
            return Ok(new { user.UserId, user.Username, user.Token });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]UserModel userModel)
        {
            try 
            {
                _userService.Create(userModel);
                return Ok();
            } 
            catch(AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody]UserModel userModel)
        {
            var user = _userService.GetById(id);

            try 
            {
                _userService.Update(user, userModel.Password);
                return Ok();
            } 
            catch(AppException ex)
            {
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