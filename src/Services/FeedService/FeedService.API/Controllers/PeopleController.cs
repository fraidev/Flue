using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FeedService.Domain.Commands.PersonCommands;
using FeedService.Domain.Repositories;
using FeedService.Infrastructure.CQRS;
using FeedService.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FeedService.Controllers
{
    [ExcludeFromCodeCoverage]
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PeopleController : ControllerBase
    {
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IPersonRepository _personRepository;

        public PeopleController(IMediatorHandler mediatorHandler, IPersonRepository personRepository)
        {
            _mediatorHandler = mediatorHandler;
            _personRepository = personRepository;
        }

        [HttpGet("")]
        public IActionResult GetPeople(string? searchText = null, int page = 1, int itemsPerPage = 10,
            SearchPeopleType searchPeopleType = SearchPeopleType.All, Guid? personId = null)
        {
            var me = _personRepository.GetByUserId(this.GetUserId());
            var people = _personRepository.GetAll();

            if (personId.HasValue)
            {
                var person = _personRepository.GetById(personId.Value);
                switch (searchPeopleType)
                {
                    case SearchPeopleType.Followers:
                        people = _personRepository.GetAll().Where(x => x.Following.Contains(person));
                        break;
                    case SearchPeopleType.Followings:
                        people = _personRepository.GetAll().Where(x => x.Followers.Contains(person));
                        break;
                    case SearchPeopleType.All:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(searchPeopleType), searchPeopleType, null);
                }
            }

            if (!string.IsNullOrEmpty(searchText))
                people = people.Where(x => x.Name.Contains(searchText)
                                           || x.Username.Contains(searchText));

            foreach (var person in people) person.IsFollowing = me.Following.Contains(person);

            people = people.Skip((page - 1) * itemsPerPage).Take(itemsPerPage);
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

    public enum SearchPeopleType
    {
        All,
        Followings,
        Followers
    }
}