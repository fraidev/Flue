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
        private readonly IPersonReadRepository _personReadRepository;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly AppSettings _appSettings;

        public PeopleController(IOptions<AppSettings> appSettings, 
            IPersonReadRepository personReadRepository,
            IMediatorHandler mediatorHandler)
        {
            _personReadRepository = personReadRepository;
            _mediatorHandler = mediatorHandler;
            _appSettings = appSettings.Value;
        }
        
        [HttpPost("Follow/{id}")]
        public IActionResult Follow(Guid id)
        {
            var person = _personReadRepository.GetByUserId(this.GetUserId());
            var cmd = new FollowPersonCommand()
            {
                PersonId = person.PersonId,
                FollowId = id
            };
            _mediatorHandler.SendCommand(cmd);
            return Ok();
        }

        [HttpPost("Unfollow/{id}")]
        public IActionResult Unfollow(Guid id)
        {
            var person = _personReadRepository.GetByUserId(this.GetUserId());
            var cmd = new UnfollowPersonCommand()
            {
                PersonId = person.PersonId,
                UnfollowId = id
            };
            _mediatorHandler.SendCommand(cmd);
            return Ok();
        }

        [HttpGet("Followers")]
        public IActionResult Followers()
        {
            var user = _personReadRepository.GetByUserId(this.GetUserId());
            var followers = _personReadRepository.GetAll().Where(x => x.Following.Contains(user));
            return Ok(followers);
        }

        [HttpGet("Following")]
        public IActionResult Following()
        {
            var user = _personReadRepository.GetByUserId(this.GetUserId());
            var following = user?.Following;
            return Ok(following);
        }
        
        [HttpGet]
        public IActionResult GetPeople(string searchText)
        {
            searchText = searchText.ToLower();
            var people =  _personReadRepository.GetAll().Where(x => x.Name.ToLower().Contains(searchText)
                || x.Username.ToLower().Contains(searchText));
            return Ok(people);
        }
//        
        [HttpGet("GetAll")]
        [Authorize(Roles = Role.Admin)]
        public IActionResult GetAll()
        {
            var people = _personReadRepository.GetAll();
            return Ok(people);
        }
    }
}