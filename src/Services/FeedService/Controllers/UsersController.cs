using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using FeedService.Domain.Read.Repositories;
using FeedService.Domain.Write.CommandHandlers;
using FeedService.Domain.Write.Commands.User;
using FeedService.Domain.Write.Repositories;
using FeedService.Infrastructure;
using FeedService.Infrastructure.CQRS;
using FeedService.Infrastructure.Extensions;
using FlueShared.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;


namespace FeedService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserReadRepository _userReadRepository;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly AppSettings _appSettings;

        public UsersController(IOptions<AppSettings> appSettings, 
            IUserRepository userRepository, IUserReadRepository userReadRepository,
            IMediatorHandler mediatorHandler)
        {
            _userRepository = userRepository;
            _userReadRepository = userReadRepository;
            _mediatorHandler = mediatorHandler;
            _appSettings = appSettings.Value;
        }
        
        [HttpPost("Follow/{id}")]
        public IActionResult Follow(Guid id)
        {
            var cmd = new FollowUserCommand()
            {
                UserId = this.GetIdentify(),
                FollowId = id
            };
            _mediatorHandler.SendCommand(cmd);
            return Ok();
        }

        [HttpPost("Unfollow/{id}")]
        public IActionResult Unfollow(Guid id)
        {
            
            var cmd = new UnfollowUserCommand()
            {
                UserId = this.GetIdentify(),
                UnfollowId = id
            };
            _mediatorHandler.SendCommand(cmd);
            return Ok();
        }

        [HttpGet("Followers")]
        public IActionResult Followers()
        {
            var user = _userReadRepository.GetById(this.GetIdentify());
            var followers = _userReadRepository.GetAll().Where(x => x.Following.Contains(user));
//            var userDtos = _mapper.Map<IList<UserCommand>>(followers);
            return Ok(followers);
        }

        [HttpGet("Following")]
        public IActionResult Following()
        {
            var following = _userReadRepository.GetById(this.GetIdentify()).Following;
//            var userDtos = _mapper.Map<IList<UserCommand>>(user.Following);
            return Ok(following);
        }
        
        [HttpGet]
        public IActionResult GetUsers(string searchText)
        {
            searchText = searchText.ToLower();
            var users =  _userReadRepository.GetAll().Where(x => x.Name.Contains(searchText)
                || x.Username.ToLower().Contains(searchText));
//            var userDtos = _mapper.Map<IList<UserCommand>>(users);
            return Ok(users);
        }
        
        [HttpGet]
        [Authorize(Roles = Role.Admin)]
        public IActionResult GetAll()
        {
            var users = _userReadRepository.GetAll();
//            var userDtos = _mapper.Map<IList<UserCommand>>(users);
            return Ok(users);
        }

        
    }
}