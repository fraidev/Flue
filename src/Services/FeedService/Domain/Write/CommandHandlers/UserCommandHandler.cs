using System;
using System.Threading;
using System.Threading.Tasks;
using FeedService.Domain.Read.Repositories;
using FeedService.Domain.Write.Aggregates;
using FeedService.Domain.Write.Commands.User;
using FeedService.Domain.Write.Repositories;
using FlueShared;
using MediatR;

namespace FeedService.Domain.Write.CommandHandlers
{
    public class UserCommandHandler:
        IRequestHandler<CreateUserCommand>,
        IRequestHandler<FollowUserCommand>,
        IRequestHandler<UnfollowUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserReadRepository _userReadRepository;

        public UserCommandHandler(IUserRepository userRepository, IUserReadRepository userReadRepository)
        {
            _userRepository = userRepository;
            _userReadRepository = userReadRepository;
        }
        
        public Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var aggregate = new UserAggregate(request);
            _userRepository.Save(aggregate);

            return Unit.Task;
        }

        public Task<Unit> Handle(FollowUserCommand request, CancellationToken cancellationToken)
        {
            var user = _userRepository.GetById(request.UserId);
            var follow = _userRepository.GetById(request.FollowId);
            
            if (user.Following.Contains(follow))
            {
                throw new Exception("Esse usuario já esta sendo seguido");
            };
            var aggregate = new UserAggregate(user);
            aggregate.Follow(user);
            
            _userRepository.Update(aggregate);

            return Unit.Task;
        }

        public Task<Unit> Handle(UnfollowUserCommand request, CancellationToken cancellationToken)
        {
            var user = _userRepository.GetById(request.UserId);
            var unfollow = _userRepository.GetById(request.UnfollowId);

            if (!user.Following.Contains(unfollow))
            {
                throw new Exception("Esse não está sendo seguido");
            };
            var aggregate = new UserAggregate(user);
            aggregate.Follow(user);
            
            _userRepository.Update(aggregate);

            return Unit.Task;
        }
    }
}