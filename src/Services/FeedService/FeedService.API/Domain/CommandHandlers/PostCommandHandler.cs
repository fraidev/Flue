using System;
using System.Threading;
using System.Threading.Tasks;
using FeedService.Domain.Aggregates;
using FeedService.Domain.Commands.Post;
using FeedService.Domain.Commands.Post.Comment;
using FeedService.Domain.Repositories;
using FeedService.Domain.States;
using MediatR;

namespace FeedService.Domain.CommandHandlers
{
    public class PostCommandHandler:
        IRequestHandler<CreatePost>,
        IRequestHandler<DeletePost>,
        IRequestHandler<AddComment>,
        IRequestHandler<UpdateComment>,
        IRequestHandler<DeleteComment>
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

        public Task<Unit> Handle(DeletePost request, CancellationToken cancellationToken)
        {
            var aggregate = _postRepository.GetAggregateById(request.Id);

            if (aggregate.GetState().Person.PersonId != request.PersonId)
            {
                throw new Exception("Não é possivel deletar um post de outro usuario");
            }
                
            aggregate.Delete();
            _postRepository.Save(aggregate);
            return Unit.Task;
        }

        public Task<Unit> Handle(AddComment request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<Unit> Handle(UpdateComment request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<Unit> Handle(DeleteComment request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}