using System;
using System.Threading;
using System.Threading.Tasks;
using FeedService.Domain.Commands.PostCommands;
using FeedService.Domain.Commands.PostCommands.Comment;
using FeedService.Domain.Repositories;
using FeedService.Infrastructure.InfraServices;
using MediatR;

namespace FeedService.Domain.CommandHandlers
{
    public class PostCommandHandler :
        IRequestHandler<CreatePost>,
        IRequestHandler<DeletePost>,
        IRequestHandler<AddComment>,
        IRequestHandler<RemoveComment>
    {
        private readonly IPersonRepository _personRepository;
        private readonly IPostRepository _postRepository;
        private readonly IUserService _userService;

        public PostCommandHandler(IPersonRepository personRepository,
            IPostRepository postRepository,
            IUserService userService)
        {
            _personRepository = personRepository;
            _postRepository = postRepository;
            _userService = userService;
        }

        public Task<Unit> Handle(AddComment request, CancellationToken cancellationToken)
        {
            var commentator = _personRepository.GetByUserId(_userService.UserId);
            var post = _postRepository.GetById(request.PostId);
            var aggregate = _personRepository.GetAggregateById(post.Person.PersonId);
            aggregate.AddComment(request, commentator);

            _personRepository.Save(aggregate);
            return Unit.Task;
        }


        public Task<Unit> Handle(CreatePost request, CancellationToken cancellationToken)
        {
            var person = _personRepository.GetAggregateByUserId(_userService.UserId);
            person.AddPost(request);
            _personRepository.Save(person);

            return Unit.Task;
        }

        public Task<Unit> Handle(DeletePost request, CancellationToken cancellationToken)
        {
            var userId = _userService.UserId;
            var aggregate = _personRepository.GetAggregateByUserId(userId);

            if (aggregate.GetState().UserId != userId)
                throw new Exception("Não é possivel deletar um post de outro usuario");

            aggregate.DeletePost(request.Id);
            _personRepository.Save(aggregate);
            return Unit.Task;
        }

        public Task<Unit> Handle(RemoveComment request, CancellationToken cancellationToken)
        {
            var userId = _userService.UserId;

            var post = _postRepository.GetById(request.PostId);

            var aggregate = _personRepository.GetAggregateById(post.Person.PersonId);

            if (aggregate.GetComment(request.PostId, request.CommentId)?.Person.UserId != userId)
                throw new Exception("Não é possivel deletar um comentario de outro usuario");

            aggregate.DeleteComment(request);

            _personRepository.Save(aggregate);
            return Unit.Task;
        }
    }
}