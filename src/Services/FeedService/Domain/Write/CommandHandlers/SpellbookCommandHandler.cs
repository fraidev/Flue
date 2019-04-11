//using System.Threading;
//using System.Threading.Tasks;
//using FeedService.Domain.Write.Aggregates;
//using FeedService.Domain.Write.Commands;
//using FeedService.Domain.Write.Repositories;
//using FeedService.Domain.Write.States;
//using MediatR;

//namespace FeedService.Domain.Write.CommandHandlers
//{
//    public class SpellbookCommandHandler:
//        IRequestHandler<CreateSpellbook>,
//        IRequestHandler<UpdateSpellbook>,
//        IRequestHandler<DeleteSpellbooks>,
//        IRequestHandler<AddSpell>,
//        IRequestHandler<UpdateSpell>,
//        IRequestHandler<DeleteSpells>
//    {
//        private readonly ISpellbookRepository _spellbookRepository;
//        private readonly ISpellbookReadRepository _spellbookReadRepository;
//        private readonly ISpellReadRepository _spellReadRepository;
//        private readonly IUserRepository _userRepository;

//        public SpellbookCommandHandler(ISpellbookRepository spellbookRepository,
//            ISpellbookReadRepository spellbookReadRepository,
//            ISpellReadRepository spellReadRepository,
//            IUserRepository userRepository)
//        {
//            _spellbookRepository = spellbookRepository;
//            _spellbookReadRepository = spellbookReadRepository;
//            _spellReadRepository = spellReadRepository;
//            _userRepository = userRepository;
//        }

//        public Task<Unit> Handle(CreateSpellbook request, CancellationToken cancellationToken)
//        {
//            var state = new SpellbookState()
//            {
//                Id = request.Id,
//                Name = request.Name,
//                Description = request.Description,
//                User = _userRepository.GetByUserProviderId(request.UserProviderId)
//            };
//            _spellbookRepository.Save(new SpellbookAggregate(state));
//            return Unit.Task;
//        }

//        public Task<Unit> Handle(UpdateSpellbook request, CancellationToken cancellationToken)
//        {

//            var state = _spellbookRepository.GetById(request.Id);
//            var aggregate = new SpellbookAggregate(state);
//            aggregate.Update(request);
//            _spellbookRepository.Update(aggregate);
//            return Unit.Task;
//        }

//        public Task<Unit> Handle(DeleteSpellbooks request, CancellationToken cancellationToken)
//        {
//            foreach (var id in request.Ids)
//            {
//                _spellbookRepository.Delete(id);
//            }
//            return Unit.Task;
//        }

//        public Task<Unit> Handle(AddSpell request, CancellationToken cancellationToken)
//        {
//            var state = _spellbookRepository.GetById(request.SpellbookId);
//            var aggregate = new SpellbookAggregate(state);
//            aggregate.AddSpell(request);
//            _spellbookRepository.Update(aggregate);
//            return Unit.Task;
//        }

//        public Task<Unit> Handle(UpdateSpell request, CancellationToken cancellationToken)
//        {
//            var spell = _spellReadRepository.GetById(request.Id);
//            var state = _spellbookRepository.GetById(spell.Spellbook.Id);
//            var aggregate = new SpellbookAggregate(state);
//            aggregate.ChangeSpell(request);
//            _spellbookRepository.Update(aggregate);
//            return Unit.Task;
//        }

//        public Task<Unit> Handle(DeleteSpells request, CancellationToken cancellationToken)
//        {
//            foreach (var id in request.Ids)
//            {
//                var spell = _spellReadRepository.GetById(id);
//                var state = _spellbookRepository.GetById(spell.Spellbook.Id);
//                var aggregate = new SpellbookAggregate(state);
//                aggregate.DeleteSpell(request);
//                _spellbookRepository.Update(aggregate);
//            }
//            return Unit.Task;
//        }
//    }
//}