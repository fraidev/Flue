using System;
using System.Collections.Generic;
using System.Linq;
using FeedService.Domain.Write.Commands;
using FeedService.Domain.Write.States;
using FeedService.Infrastructure.CQRS;

namespace FeedService.Domain.Write.Aggregates
{
    public class PostAggregate:IBaseAggregate<PostState>
    {
        #region Properties

        public Guid Id { get; }
        private PostState State { get; set; }
        
        #endregion

        #region Constructors
        
        public PostAggregate(PostState state)
        {
            Id = Guid.NewGuid();
            State = state;
        }
        
        public PostAggregate(CreatePost cmd)
        {
            Id = Guid.NewGuid();
            State = new PostState()
            {
                PostId = Id,
                PersonId = cmd.PersonId,
                Text = cmd.Text,
                Comments = new List<CommentState>()
            };
        }
        
        #endregion

        #region Operations

        public PostState GetState()
        {
            return State;
        }
        
        public void Update(UpdatePost cmd)
        {
            State.Text = cmd.Text;
        }
        
        public void Delete()
        {
            State.Deleted = true;
        }

        public void AddComment(AddComment cmd)
        {
            State.Comments.Add(new CommentState()
            {
                Id = cmd.Id,
                Text = cmd.Text,
                PersonId = cmd.PersonId,
                CommentReply = cmd.CommentReply
            });
        }
        
        public void UpdateComment(UpdateComment cmd)
        {
            var comments = State.Comments.FirstOrDefault(x => x.Id == cmd.Id);

            if (comments != null)
            {
                comments.Text = cmd.Text;
            }
        }

        public void DeleteComments(DeleteComments cmd)
        {
            foreach (var id in cmd.Ids)
            {
                var spell = State.Comments.FirstOrDefault(x => x.Id == id);
                State.Comments.Remove(spell);
            }
        }
        
        #endregion
    }
}