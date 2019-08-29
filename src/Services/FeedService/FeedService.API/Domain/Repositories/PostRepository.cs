using System;
using System.Collections.Generic;
using System.Linq;
using FeedService.Domain.States;
using FeedService.Infrastructure.Persistence;

namespace FeedService.Domain.Repositories
{
    public interface IPostRepository
    {
        Post GetById(Guid id);
        Post GetByIdAndPersonId(Guid id, Guid personId);
        Comment GetCommentById(Guid id);
        void Delete(Guid id);
        IQueryable<Post> GetAll();
        IEnumerable<Post> GetMyFeed(Person person);
        IEnumerable<Post> GetMyPosts(Guid personId);
    }

    public class PostRepository : IPostRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public PostRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Post GetById(Guid id)
        {
            return _unitOfWork.GetById<Post>(id);
        }

        public Post GetByIdAndPersonId(Guid id, Guid personId)
        {
            var post = _unitOfWork.GetById<Post>(id);
            MarkMyPostAndComment(post, personId);

            return post;
        }

        public Comment GetCommentById(Guid id)
        {
            return _unitOfWork.GetById<Comment>(id);
        }

        public void Delete(Guid id)
        {
            _unitOfWork.Delete(GetById(id));
            _unitOfWork.Flush();
        }

        public IQueryable<Post> GetAll()
        {
            return _unitOfWork.Query<Post>();
        }

        public IEnumerable<Post> GetMyFeed(Person person)
        {
            if (person == null) throw new Exception();

            var followersPersonId = person.Following.Select(x => x.PersonId);
            var posts = _unitOfWork.Query<Post>()
                .Where(x => followersPersonId.Contains(x.Person.PersonId) &&
                            !x.Deleted);

            posts = MarkMyPostsAndComments(posts, person.PersonId);

            posts = posts.OrderByDescending(x => x.CreatedDate);

            return posts;
        }

        public IEnumerable<Post> GetMyPosts(Guid personId)
        {
            var posts = _unitOfWork.Query<Post>().Where(x => !x.Deleted && x.Person.PersonId == personId);

            posts = MarkMyPostsAndComments(posts, personId);

            return posts.OrderByDescending(x => x.CreatedDate);
        }

        private static IQueryable<Post> MarkMyPostsAndComments(IQueryable<Post> posts, Guid personId)
        {
            foreach (var post in posts) MarkMyPostAndComment(post, personId);

            return posts;
        }

        private static void MarkMyPostAndComment(Post post, Guid personId)
        {
            post.IsMyPost = post.Person.PersonId == personId;
            foreach (var comment in post.Comments) comment.IsMyComment = comment.Person.PersonId == personId;
        }
    }
}