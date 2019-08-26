using System;
using System.Collections.Generic;
using System.Linq;
using FeedService.Domain.Commands.PostCommands;
using FeedService.Domain.Commands.PostCommands.Comment;
using FeedService.Domain.States;
using FeedService.Infrastructure.CQRS;
using FeedService.Infrastructure.Extensions;
using FlueShared;

namespace FeedService.Domain.Aggregates
{
    public class PersonAggregate : IBaseAggregate<Person>
    {
        private Person State { get; }
        public Guid Id { get; }

        public Person GetState()
        {
            return State;
        }

        public void AddPost(CreatePost cmd)
        {
            var post = new Post
            {
                PostId = cmd.Id,
                Person = GetState(),
                Text = cmd.Text,
                Comments = new List<Comment>(),
                CreatedDate = DateTime.Now
            };
            GetState().Posts.Add(post);
        }

        public Post GetPost(Guid postId)
        {
            return GetState().Posts.FirstOrDefault(x => x.PostId == postId);
        }

        public Comment GetComment(Guid postId, Guid commentId)
        {
            return GetState().Posts.FirstOrDefault(x => x.PostId == postId)
                ?.Comments.FirstOrDefault(x => x.CommentId == commentId);
        }

        public void AddPost(Post post)
        {
            GetState().Posts.Add(post);
        }

        public void Follow(Person person)
        {
            GetState().Following.Add(person);
        }

        public void Unfollow(Person person)
        {
            GetState().Following = GetState().Following.Where(x => x.PersonId != person.PersonId).ToList();
        }

        public void AddComment(AddComment cmd, Person commentator)
        {
            var post = GetState().Posts.FirstOrDefault(x => x.PostId == cmd.PostId);
            post?.Comments.Add(new Comment
            {
                CommentId = cmd.Id,
                Text = cmd.Text,
                Person = commentator,
                CreatedDate = DateTime.Now,
                Post = post
            });
        }

        public void DeletePost(Guid postId)
        {
            var post = GetState().Posts.FirstOrDefault(x => x.PostId == postId);
            if (post != null) post.Deleted = true;
        }

        public void DeleteComment(RemoveComment cmd)
        {
            GetState().Posts.FirstOrDefault(x => x.PostId == cmd.PostId)?.Comments
                .Remove(GetComment(cmd.PostId, cmd.CommentId));
        }


        #region Constructors

        public PersonAggregate(Person state)
        {
            Id = Guid.NewGuid();
            State = state;
        }

        public PersonAggregate(CreatePersonCommand cmd)
        {
            Id = Guid.NewGuid();
            State = new Person
            {
                PersonId = Id,
                UserId = cmd.IdentifierId,
                Username = cmd.Username,
                Name = cmd.Name.Capitalize(),
                Description = "Eu sou novo no Flue!",
                Email = cmd.Email,
                Following = new List<Person>(),
                Followers = new List<Person>()
            };
        }

        #endregion
    }
}