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
    public class PeopleController : ControllerBase
    {
        private readonly IPersonRepository _personRepository;
        private readonly IPersonReadRepository _personReadRepository;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly AppSettings _appSettings;

        public PeopleController(IOptions<AppSettings> appSettings, 
            IPersonRepository personRepository, IPersonReadRepository personReadRepository,
            IMediatorHandler mediatorHandler)
        {
            _personRepository = personRepository;
            _personReadRepository = personReadRepository;
            _mediatorHandler = mediatorHandler;
            _appSettings = appSettings.Value;
        }
        
        [HttpPost("Follow/{id}")]
        public IActionResult Follow(Guid id)
        {
            var cmd = new FollowPersonCommand()
            {
                PersonId = this.GetIdentify(),
                FollowId = id
            };
            _mediatorHandler.SendCommand(cmd);
            return Ok();
        }

        [HttpPost("Unfollow/{id}")]
        public IActionResult Unfollow(Guid id)
        {
            var cmd = new UnfollowPersonCommand()
            {
                PersonId = this.GetIdentify(),
                UnfollowId = id
            };
            _mediatorHandler.SendCommand(cmd);
            return Ok();
        }

        [HttpGet("Followers")]
        public IActionResult Followers()
        {
            var user = _personReadRepository.GetById(this.GetIdentify());
            var followers = _personReadRepository.GetAll().Where(x => x.Following.Contains(user));
//            var userDtos = _mapper.Map<IList<UserCommand>>(followers);
            return Ok(followers);
        }

        [HttpGet("Following")]
        public IActionResult Following()
        {
            var following = _personReadRepository.GetById(this.GetIdentify()).Following;
//            var userDtos = _mapper.Map<IList<UserCommand>>(user.Following);
            return Ok(following);
        }
        
        [HttpGet]
        public IActionResult GetUsers(string searchText)
        {
            searchText = searchText.ToLower();
            var people =  _personReadRepository.GetAll().Where(x => x.Name.Contains(searchText)
                || x.Username.ToLower().Contains(searchText));
//            var userDtos = _mapper.Map<IList<UserCommand>>(users);
            return Ok(people);
        }
        
        [HttpGet]
        [Authorize(Roles = Role.Admin)]
        public IActionResult GetAll()
        {
            var people = _personReadRepository.GetAll();
//            var userDtos = _mapper.Map<IList<UserCommand>>(users);
            return Ok(people);
        }
    }
}