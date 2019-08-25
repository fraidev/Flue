using System;
using System.Threading;
using System.Threading.Tasks;
using FeedService.Domain.Aggregates;
using FeedService.Domain.Commands.PersonCommands;
using FeedService.Domain.Repositories;
using FlueShared;
using MediatR;

namespace FeedService.Domain.CommandHandlers
{
    public class PersonCommandHandler:
        IRequestHandler<CreatePersonCommand>,
        IRequestHandler<FollowPersonCommand>,
        IRequestHandler<UnfollowPersonCommand>
    {
        private readonly IPersonRepository _personRepository;

        public PersonCommandHandler(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }
        
        public Task<Unit> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
        {
            var aggregate = new PersonAggregate(request);
            _personRepository.Save(aggregate);

            return Unit.Task;
        }

        public Task<Unit> Handle(FollowPersonCommand request, CancellationToken cancellationToken)
        {
            var user = _personRepository.GetById(request.PersonId);
            var follow = _personRepository.GetById(request.FollowId);
            
            if (user.Following.Contains(follow))
            {
                throw new Exception("Esse usuario já esta sendo seguido");
            };
            var aggregate = new PersonAggregate(user);
            aggregate.Follow(follow);
            
            _personRepository.Update(aggregate);

            return Unit.Task;
        }

        public Task<Unit> Handle(UnfollowPersonCommand request, CancellationToken cancellationToken)
        {
            var user = _personRepository.GetById(request.PersonId);
            var unfollow = _personRepository.GetById(request.UnfollowId);

            if (!user.Following.Contains(unfollow))
            {
                throw new Exception("Esse não está sendo seguido");
            };
            var aggregate = new PersonAggregate(user);
            aggregate.Unfollow(unfollow);
            
            _personRepository.Update(aggregate);

            return Unit.Task;
        }
    }
}