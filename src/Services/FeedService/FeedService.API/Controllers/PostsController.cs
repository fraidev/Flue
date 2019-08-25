using System;
using System.Linq;
using System.Security.Claims;
using FeedService.Domain.Commands.PostCommands;
using FeedService.Domain.Commands.PostCommands.Comment;
using FeedService.Domain.Repositories;
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
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IPersonRepository _personRepository;
        private readonly IPostRepository _postRepository;

        public PostsController(IMediatorHandler mediatorHandler,
            IPostRepository postRepository,
            IPersonRepository personRepository)
        {
            _mediatorHandler = mediatorHandler;
            _postRepository = postRepository;
            _personRepository = personRepository;
        }

        [HttpGet("Identity")]
        public IActionResult Identity()
        {
            var claim = (ClaimsIdentity) User.Identity;
            var nameIdentifier = claim.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value)
                .SingleOrDefault();
            var role = claim.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).SingleOrDefault();
            var name = claim.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
            return Ok(nameIdentifier + role + name);
        }

        [HttpGet("")]
        public IActionResult GetAll()
        {
            return Ok(_postRepository.GetAll());
        }

        [HttpGet("Feed")]
        public IActionResult GetMyFeed()
        {
            var me = _personRepository.GetByUserId(this.GetUserId());
            var feed = _postRepository.GetMyFeed(me);

            return Ok(feed);
        }

        [HttpGet("MyPosts")]
        public IActionResult GetMyPosts()
        {
            var me = _personRepository.GetByUserId(this.GetUserId());
            var myPosts = _postRepository.GetMyFeed(me);

            return Ok(myPosts);
        }

        [HttpGet("MyPostCount")]
        public IActionResult GetMyPostCount()
        {
            var me = _personRepository.GetByUserId(this.GetUserId());
            if (me == null)
            {
                return Ok();
            }

            var myPostsCount = _postRepository.GetMyPosts(me.PersonId).Count();
            return Ok(myPostsCount);
        }

        [HttpGet("{id}")]
        public IActionResult GetPostById(Guid id)
        {
            return Ok(_postRepository.GetById(id));
        }

        [HttpPost]
        public void Post([FromBody] CreatePost cmd)
        {
            cmd.Id = Guid.NewGuid();
            _mediatorHandler.SendCommand(cmd);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var cmd = new DeletePost
            {
                Id = id,
                UserId = this.GetUserId()
            };
            _mediatorHandler.SendCommand(cmd);
            return Ok();
        }

        [HttpPost("Comment")]
        public IActionResult AddComment([FromBody] AddComment cmd)
        {
            _mediatorHandler.SendCommand(cmd);
            return Ok();
        }

        [HttpDelete("Comment/{id}")]
        public IActionResult RemoveComment(Guid id)
        {
            var cmd = new RemoveComment
            {
                Id = id,
                UserId = this.GetUserId()
            };
            _mediatorHandler.SendCommand(cmd);
            return Ok();
        }
    }
}