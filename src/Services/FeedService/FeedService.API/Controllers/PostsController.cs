using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FeedService.Domain.Commands.PostCommands;
using FeedService.Domain.Commands.PostCommands.Comment;
using FeedService.Domain.Repositories;
using FeedService.Infrastructure.CQRS;
using FeedService.Infrastructure.InfraServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FeedService.Controllers
{
    [ExcludeFromCodeCoverage]
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IPersonRepository _personRepository;
        private readonly IPostRepository _postRepository;
        private readonly IUserService _userService;

        public PostsController(IMediatorHandler mediatorHandler,
            IPostRepository postRepository,
            IPersonRepository personRepository,
            IUserService userService)
        {
            _mediatorHandler = mediatorHandler;
            _postRepository = postRepository;
            _personRepository = personRepository;
            _userService = userService;
        }
        
        [HttpGet("Feed")]
        public IActionResult GetMyFeed(int page = 1, int itemsPerPage = 10)
        {
            var me = _personRepository.GetByUserId(_userService.UserId);
            var feed = _postRepository.GetMyFeed(me);

            feed = feed.Skip((page - 1) * itemsPerPage).Take(itemsPerPage);
            return Ok(feed);
        }

        [HttpGet("person/{id}")]
        public IActionResult GetPostsByPersonId(Guid id, int page = 1, int itemsPerPage = 10)
        {
            var myPosts = _postRepository.GetPostsByPersonId(id);
            
            myPosts = myPosts.Skip((page - 1) * itemsPerPage).Take(itemsPerPage);
            return Ok(myPosts);
        }

        [HttpGet("{id}")]
        public IActionResult GetPostById(Guid id)
        {
            var me = _personRepository.GetByUserId(_userService.UserId); 
            return Ok(_postRepository.GetByIdAndPersonId(id, me.PersonId));
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

        [HttpPost("RemoveComment")]
        public IActionResult RemoveComment(RemoveComment cmd)
        {
            _mediatorHandler.SendCommand(cmd);
            return Ok();
        }
    }
}