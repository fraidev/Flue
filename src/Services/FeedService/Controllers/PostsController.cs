using System;
using System.Linq;
using System.Security.Claims;
using FeedService.Domain.Read.Repositories;
using FeedService.Domain.Write.Aggregates;
using FeedService.Domain.Write.Commands;
using FeedService.Domain.Write.Repositories;
using FeedService.Infrastructure.Broker;
using FeedService.Infrastructure.CQRS;
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
        private readonly IFeedRepository _feedRepository;
        private readonly IPostReadRepository _postReadRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public PostsController(IFeedRepository feedRepository, IPostReadRepository postReadRepository, IMediatorHandler mediatorHandler)
        {
            _feedRepository = feedRepository;
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
        
        [HttpGet("Inbox")]
        public IActionResult Inbox()
        {
            var userId = Guid.Parse(((ClaimsIdentity) User.Identity).Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                .Select(c => c.Value).SingleOrDefault());
            
            // Meus seguidores
            
            return Ok(_postReadRepository.GetAll());
        }
        
        [HttpGet("{id}")]
        public IActionResult GetPostById(Guid id)
        {
            return Ok(_postReadRepository.GetById(id));
        }

        [HttpPost]
        public void Post([FromBody] CreatePost cmd)
        {
//            var cm = new CreateUserCommand {UserId = new Guid()};
            
            cmd.UserId = Guid.Parse(((ClaimsIdentity) User.Identity).Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                .Select(c => c.Value).SingleOrDefault());
            _mediatorHandler.SendCommand(cmd);
//
//            var f = new MessageBroker();
//            var r = f.Call("30");
//            Console.WriteLine(r);
//            f.Close();
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
