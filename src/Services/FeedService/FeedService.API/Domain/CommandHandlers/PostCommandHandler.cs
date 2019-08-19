using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FeedService.Domain.Aggregates;
using FeedService.Domain.Commands.PostCommands;
using FeedService.Domain.Commands.PostCommands.Comment;
using FeedService.Domain.Repositories;
using FeedService.Domain.States;
using MediatR;

namespace FeedService.Domain.CommandHandlers
{
    public class PostCommandHandler:
        IRequestHandler<CreatePost>,
        IRequestHandler<DeletePost>,
        IRequestHandler<AddComment>,
        IRequestHandler<RemoveComment>
    {
        private readonly IPersonRepository _personRepository;

        public PostCommandHandler(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public Task<Unit> Handle(CreatePost request, CancellationToken cancellationToken)
        {
            var person = _personRepository.GetAggregateById(request.Person.PersonId);
            person.AddPost(request);
            _personRepository.Save(person);
            return Unit.Task;
        }

        public Task<Unit> Handle(DeletePost request, CancellationToken cancellationToken)
        {
            var aggregate = _personRepository.GetAggregateById(request.UserId);

            if (aggregate.GetState().UserId != request.UserId)
            {
                throw new Exception("Não é possivel deletar um post de outro usuario");
            }
                
            aggregate.DeletePost(request.Id);
            _personRepository.Save(aggregate);
            return Unit.Task;
        }

        public Task<Unit> Handle(AddComment request, CancellationToken cancellationToken)
        {
            var aggregate = _personRepository.GetAggregateById(request.Person.UserId);
            aggregate.AddComment(request);
            
            _personRepository.Save(aggregate);
            return Unit.Task;
        }

        public Task<Unit> Handle(RemoveComment request, CancellationToken cancellationToken)
        {
            var aggregate = _personRepository.GetAggregateById(request.UserId);
            
            if (aggregate.GetComment(request.UserId, request.Id)
                    ?.Person.UserId != request.UserId)
            {
                throw new Exception("Não é possivel deletar um post de outro usuario");
            }
            
            aggregate.DeleteComment(request);
            
            _personRepository.Save(aggregate);
            return Unit.Task;
        }
    }
}