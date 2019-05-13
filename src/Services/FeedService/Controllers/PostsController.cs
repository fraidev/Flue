using System;
using System.Linq;
using System.Security.Claims;
using FeedService.Domain.Read.Repositories;
using FeedService.Domain.Write.Aggregates;
using FeedService.Domain.Write.Commands;
using FeedService.Domain.Write.Repositories;
using FeedService.Infrastructure.Broker;
using FeedService.Infrastructure.CQRS;
using FeedService.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FeedService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IPersonReadRepository _personReadRepository;
        private readonly IPostReadRepository _postReadRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public PostsController(IPersonReadRepository personReadRepository, IPostReadRepository postReadRepository, IMediatorHandler mediatorHandler)
        {
            _personReadRepository = personReadRepository;
            _postReadRepository = postReadRepository;
            _mediatorHandler = mediatorHandler;
        }
        
        [HttpGet("Identity")]
        public IActionResult Identity()
        {
            var claim = ((ClaimsIdentity)User.Identity);
            var nameIdentifier = claim.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).SingleOrDefault();
            var role = claim.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).SingleOrDefault();
            var name = claim.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
            return Ok(nameIdentifier + role + name);
        }
        
        [HttpGet("")]
        public IActionResult GetAll()
        {
            return Ok(_postReadRepository.GetAll());
        }
        
        [HttpGet("Feed")]
        public IActionResult GetMyFeed()
        {
            var me = _personReadRepository.GetByUserId(this.GetUserId());
            var feed = _postReadRepository.GetMyFeed(me);
            
            return Ok(feed);
        }
        
        [HttpGet("{id}")]
        public IActionResult GetPostById(Guid id)
        {
            return Ok(_postReadRepository.GetById(id));
        }

        [HttpPost]
        public void Post([FromBody] CreatePost cmd)
        {
            cmd.PersonId = _personReadRepository.GetByUserId(this.GetUserId()).PersonId;
            _mediatorHandler.SendCommand(cmd);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
