using System.Threading;
using System.Threading.Tasks;
using FeedService.Domain.Aggregates;
using FeedService.Domain.Commands.Post;
using FeedService.Domain.Repositories;
using MediatR;

namespace FeedService.Domain.CommandHandlers
{
    public class PostCommandHandler:
        IRequestHandler<CreatePost>,
        IRequestHandler<UpdatePost>,
        IRequestHandler<DeletePosts>,
        IRequestHandler<AddComment>,
        IRequestHandler<UpdateComment>,
        IRequestHandler<DeleteComments>
    {
        private readonly IPostRepository _postRepository;

        public PostCommandHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public Task<Unit> Handle(CreatePost request, CancellationToken cancellationToken)
        {
            _postRepository.Save(new PostAggregate(request));
            return Unit.Task;
        }

        public Task<Unit> Handle(UpdatePost request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<Unit> Handle(DeletePosts request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<Unit> Handle(AddComment request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<Unit> Handle(UpdateComment request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<Unit> Handle(DeleteComments request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}