using System;
using System.Linq;
using FeedService.Domain.Commands.Person;
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
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IPersonRepository _personRepository;
        private readonly AppSettings _appSettings;

        public PeopleController(IOptions<AppSettings> appSettings, 
            IMediatorHandler mediatorHandler,
            IPersonRepository personRepository)
        {
            _mediatorHandler = mediatorHandler;
            _personRepository = personRepository;
            _appSettings = appSettings.Value;
        }
        
        [HttpPost("Follow/{id}")]
        public IActionResult Follow(Guid id)
        {
            var person = _personRepository.GetByUserId(this.GetUserId());
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
            var person = _personRepository.GetByUserId(this.GetUserId());
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
            var person = _personRepository.GetByUserId(this.GetUserId());
            var followers = _personRepository.GetAll().Where(x => x.Following.Contains(person));
            return Ok(followers);
        }

        [HttpGet("Following")]
        public IActionResult Following()
        {
            var person = _personRepository.GetByUserId(this.GetUserId());
            var following = person?.Following;
            return Ok(following);
        }

        [HttpGet("Me")]
        public IActionResult Me()
        {
            var person = _personRepository.GetByUserId(this.GetUserId());
            return Ok(person);
        }
        
        [HttpGet]
        public IActionResult GetPeople(string searchText)
        {
            searchText = searchText.ToLower();
            var people =  _personRepository.GetAll().Where(x => x.Name.ToLower().Contains(searchText)
                || x.Username.ToLower().Contains(searchText));
            return Ok(people);
        }
//        
        [HttpGet("GetAll")]
        [Authorize(Roles = Role.Admin)]
        public IActionResult GetAll()
        {
            var people = _personRepository.GetAll();
            return Ok(people);
        }
    }
}