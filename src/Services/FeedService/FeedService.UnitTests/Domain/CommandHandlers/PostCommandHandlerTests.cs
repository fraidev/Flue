using System;
using System.Linq;
using System.Threading;
using AutoFixture;
using FeedService.Domain.Aggregates;
using FeedService.Domain.CommandHandlers;
using FeedService.Domain.Commands.PostCommands;
using FeedService.Domain.Commands.PostCommands.Comment;
using FeedService.Domain.Repositories;
using FeedService.Domain.States;
using FeedService.Infrastructure.InfraServices;
using NSubstitute;
using Xunit;

namespace FeedService.UnitTests.Domain.CommandHandlers
{
    public class PostCommandHandlerTests : BaseUnitTest
    {
        public PostCommandHandlerTests()
        {
            _personRepository = Substitute.For<IPersonRepository>();
            _postRepository = Substitute.For<IPostRepository>();
            _userService = Substitute.For<IUserService>();

            _sut = new PostCommandHandler(_personRepository, _postRepository, _userService);
        }

        private readonly IPersonRepository _personRepository;
        private readonly IPostRepository _postRepository;
        private readonly IUserService _userService;
        private readonly PostCommandHandler _sut;

        [Fact]
        public void AddCommentMustUseRepositoryForSaveAggregate()
        {
            //Given
            var cmd = Fixture.Create<AddComment>();
            var commentator = Fixture.Create<Person>();
            var aggregate = Fixture.Create<PersonAggregate>();
            var post = Fixture.Create<Post>();
            cmd.PostId = post.PostId;
            aggregate.AddPost(post);

            _personRepository.GetByUserId(Arg.Any<Guid>()).Returns(commentator);
            _postRepository.GetById(cmd.PostId).Returns(post);
            _personRepository.GetAggregateById(Arg.Any<Guid>()).Returns(aggregate);

            //When
            _sut.Handle(cmd, CancellationToken.None);

            //Then
            _personRepository.Received(1).Save(Arg.Is<PersonAggregate>(
                x => x.GetState().Posts.First(x => x.PostId == cmd.PostId)
                    .Comments.Any(x => x.Text == cmd.Text && x.CommentId == cmd.Id)));
        }

        [Fact]
        public void CreatePostMustUseRepositoryForSaveAggregate()
        {
            //Given
            var cmd = new CreatePost();
            var aggregate = Fixture.Create<PersonAggregate>();

            _personRepository.GetAggregateByUserId(_userService.UserId).Returns(aggregate);
            
            
            //When
            _sut.Handle(cmd, CancellationToken.None);

            //Then
            _personRepository.Received(1).Save(Arg.Is<PersonAggregate>(
                x => x.GetState().Posts.Any(x => x.PostId == cmd.Id && x.Text == cmd.Text)));
        }

        [Fact]
        public void DeletePostMustUseRepositoryForSaveAggregate()
        {
            //Given
            var cmd = Fixture.Create<DeletePost>();
            var person = Fixture.Create<Person>();
            var aggregate = new PersonAggregate(person);
            
            _userService.UserId.Returns(person.UserId);
            _personRepository.GetAggregateByUserId(_userService.UserId).Returns(aggregate);
            
            
            //When
            _sut.Handle(cmd, CancellationToken.None);

            //Then
            _personRepository.Received(1).Save(Arg.Is<PersonAggregate>(
                x => x.GetState().Posts[0].Deleted));
        }
        
        [Fact]
        public void MustThrowExceptionWhenUserIdIsNotFromUserPost()
        {
            //Given
            var cmd = Fixture.Create<DeletePost>();
            var person = Fixture.Create<Person>();
            var aggregate = new PersonAggregate(person);
            var post = Fixture.Create<Post>();
            cmd.Id = post.PostId;
            post.Person = person;
            person.Posts.Clear();;
            aggregate.AddPost(post);
            
            _personRepository.GetAggregateByUserId(_userService.UserId).Returns(aggregate);

            //When and //Then
            Assert.ThrowsAsync<Exception>(() => _sut.Handle(cmd, CancellationToken.None));
        }
    }
}