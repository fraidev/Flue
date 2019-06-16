using System;
using System.Collections.Generic;
using System.Linq;
using FeedService.Domain.Commands.PostCommands;
using FeedService.Domain.Commands.PostCommands.Comment;
using FeedService.Domain.States;
using FeedService.Infrastructure.CQRS;

namespace FeedService.Domain.Aggregates
{
    public class PostAggregate:IBaseAggregate<Post>
    {
        #region Properties

        public Guid Id { get; }
        private Post State { get; set; }
        
        #endregion

        #region Constructors
        
        public PostAggregate(Post state)
        {
            Id = Guid.NewGuid();
            State = state;
        }
        
        public PostAggregate(CreatePost cmd)
        {
            Id = Guid.NewGuid();
            State = new Post()
            {
                PostId = Id,
                Person = cmd.Person,
                Text = cmd.Text,
                Comments = new List<Comment>()
            };
        }
        
        #endregion

        #region Operations

        public Post GetState()
        {
            return State;
        }
        
        public void Delete()
        {
            State.Deleted = true;
        }

        public void AddComment(AddComment cmd)
        {
            State.Comments.Add(new Comment()
            {
                CommentId = cmd.Id,
                Text = cmd.Text,
                Person = cmd.Person,
                Post = State
            });
        }

        public void DeleteComment(RemoveComment cmd)
        {
            var spell = State.Comments.FirstOrDefault(x => x.CommentId == cmd.Id);
            State.Comments.Remove(spell);
        }
        
        #endregion
    }
}