using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FeedService.Domain.Write.Aggregates;
using FeedService.Domain.Write.Commands;
using FeedService.Domain.Write.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FeedService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : Controller
    {
        private readonly IFeedRepository _feedRepository;

        public PostController(IFeedRepository feedRepository)
        {
            _feedRepository = feedRepository;
        }
        [HttpGet("Frost")]
        public IActionResult Frost()
        {
            var claim = ((ClaimsIdentity)User.Identity);
            var nameIdentifier = claim.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).SingleOrDefault();
            var role = claim.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).SingleOrDefault();
            var name = claim.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
            return Ok(nameIdentifier + role + name);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void CreatePost([FromBody] CreatePost cmd)
        {
            var aggregate = new FeedAggregate(cmd);
            _feedRepository.Save(aggregate);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
