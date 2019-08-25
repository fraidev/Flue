using System;
using System.Collections.Generic;
using System.Linq;
using FeedService.Domain.Aggregates;
using FeedService.Domain.States;
using FeedService.Infrastructure.Persistence;

namespace FeedService.Domain.Repositories
{
    public interface IPostRepository
    {
        Post GetById(Guid id);
        Comment GetCommentById(Guid id);
//        PostAggregate GetAggregateById(Guid id);
//        void Save(PostAggregate postAggregate);
        void Delete(Guid id);
//        void Update(PostAggregate postAggregate);
        IQueryable<Post> GetAll();
        IEnumerable<Post> GetMyFeed(Person person);
        IEnumerable<Post> GetMyPosts(Guid personId);
    }

    public class PostRepository: IPostRepository
    {
        private IUnitOfWork _unitOfWork;

        public PostRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Post GetById(Guid id)
        {
            return _unitOfWork.GetById<Post>(id);
        }

        public Comment GetCommentById(Guid id)
        {
            return _unitOfWork.GetById<Comment>(id);
        }

//        public PostAggregate GetAggregateById(Guid id)
//        {
//            return new PostAggregate(_unitOfWork.GetById<Post>(id));
//        }
//    
//        public void Save(PostAggregate postAggregate)
//        {
//            _unitOfWork.Save(postAggregate.GetState());
//            _unitOfWork.Flush();
//        }

        public void Delete(Guid id)
        {
            _unitOfWork.Delete(GetById(id));
            _unitOfWork.Flush();
        }
        
//        public void Update(PostAggregate postAggregate)
//        {
//            var state = postAggregate.GetState();
//            _unitOfWork.Update(state);
//            _unitOfWork.Flush();
//        }

        public IQueryable<Post> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Post> GetMyFeed(Person person)
        {
            if (person == null)
            {
                throw new Exception();
            }
            
            var followersPersonId = person.Following.Select(x => x.PersonId);
            var posts = _unitOfWork.Query<Post>()
                .Where(x => followersPersonId.Contains(x.Person.PersonId) &&
                            !x.Deleted);

            foreach (var post in posts)
            {
                post.IsMyPost = post.Person.PersonId == person.PersonId;
                foreach (var comment in post.Comments)
                {
                    comment.IsMyComment = comment.Person.PersonId == person.PersonId;
                }
            }
            
            posts = posts.OrderByDescending(x => x.CreatedDate);

            return posts;
        }

        public IEnumerable<Post> GetMyPosts(Guid personId)
        {
            return _unitOfWork.Query<Post>().Where(x => !x.Deleted && x.Person.PersonId == personId)
                .OrderByDescending(x => x.CreatedDate);
        }
    }
}