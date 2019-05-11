using System;
using System.Threading;
using System.Threading.Tasks;
using FeedService.Domain.Write.Aggregates;
using FeedService.Domain.Write.Commands;
using FeedService.Domain.Write.Repositories;
using FeedService.Domain.Write.States;
using FlueShared;
using MediatR;

namespace FeedService.Domain.Write.CommandHandlers
{
    public class PostCommandHandler:
        IRequestHandler<CreatePost>,
        IRequestHandler<UpdatePost>,
        IRequestHandler<DeletePosts>,
        IRequestHandler<AddComment>,
        IRequestHandler<UpdateComment>,
        IRequestHandler<DeleteComments>,
        IRequestHandler<CreateUserCommand>
    {
        private readonly IFeedRepository _feedRepository;

        public PostCommandHandler(IFeedRepository feedRepository)
        {
            _feedRepository = feedRepository;
        }

        public Task<Unit> Handle(CreatePost request, CancellationToken cancellationToken)
        {
            var state = new PostState()
            {
                Id = request.Id,
                Text = request.Text,
                UserId = request.UserId
            };
            _feedRepository.Save(new FeedAggregate(state));
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
        
        public Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            Console.WriteLine("Batata doce");
            return Unit.Task;
        }
    }
}