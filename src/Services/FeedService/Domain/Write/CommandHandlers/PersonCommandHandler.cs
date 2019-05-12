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
    public class PersonCommandHandler:
        IRequestHandler<CreatePersonCommand>,
        IRequestHandler<FollowPersonCommand>,
        IRequestHandler<UnfollowPersonCommand>
    {
        private readonly IPersonRepository _personRepository;
        private readonly IPersonReadRepository _personReadRepository;

        public PersonCommandHandler(IPersonRepository personRepository, IPersonReadRepository personReadRepository)
        {
            _personRepository = personRepository;
            _personReadRepository = personReadRepository;
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
            aggregate.Follow(user);
            
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
            aggregate.Follow(user);
            
            _personRepository.Update(aggregate);

            return Unit.Task;
        }
    }
}