using System;
using System.Linq;
using FeedService.Domain.Commands.PersonCommands;
using FeedService.Domain.Repositories;
using FeedService.Infrastructure;
using FeedService.Infrastructure.CQRS;
using FeedService.Infrastructure.Extensions;
using FlueShared.Entities;
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
        private readonly AppSettings _appSettings;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IPersonRepository _personRepository;

        public PeopleController(IOptions<AppSettings> appSettings,
            IMediatorHandler mediatorHandler,
            IPersonRepository personRepository)
        {
            _mediatorHandler = mediatorHandler;
            _personRepository = personRepository;
            _appSettings = appSettings.Value;
        }

        [HttpGet("")]
        public IActionResult GetPeople(string searchText, int page = 1, int count = 10)
        {
            var me = _personRepository.GetByUserId(this.GetUserId());
            var people = _personRepository.GetAll();

            if (!string.IsNullOrEmpty(searchText))
                people = people.Where(x => x.Name.ToLower().Contains(searchText)
                                           || x.Username.ToLower().Contains(searchText));

            foreach (var person in people) person.IsFollowing = me.Following.Contains(person);
            return Ok(people);
        }

        [HttpGet("Me")]
        public IActionResult Me()
        {
            var person = _personRepository.GetByUserId(this.GetUserId());
            return Ok(person);
        }

        [HttpPost("Follow/{id}")]
        public IActionResult Follow(Guid id)
        {
            var person = _personRepository.GetByUserId(this.GetUserId());
            var cmd = new FollowPersonCommand
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
            var person = _personRepository.GetByUserId(this.GetUserId());
            var cmd = new UnfollowPersonCommand
            {
                PersonId = person.PersonId,
                UnfollowId = id
            };
            _mediatorHandler.SendCommand(cmd);
            return Ok();
        }

        [HttpPost("UpdatePerson")]
        public IActionResult UpdateProfile(UpdatePersonCommand cmd)
        {
            var person = _personRepository.GetByUserId(this.GetUserId());
            cmd.PersonId = person.PersonId;
            _mediatorHandler.SendCommand(cmd);
            return Ok();
        }
    }
}