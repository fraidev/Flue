using System;
using System.Collections.Generic;
using System.Linq;
using AccountService.Domain.Write.State;
using AccountService.Infrastructure.Persistence;

namespace AccountService.Domain.Write.Repositories
{
    public interface IUserRepository
    {
        UserState GetById(Guid id);
        UserState GetByUser(string id);
        IEnumerable<UserState> GetAll();
        void Save(UserState userState);
        void Update(UserState userState);
        void Delete(UserState userState);
    }
    public class UserRepository: IUserRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public UserState GetById(Guid id)
        {
            return _unitOfWork.GetById<UserState>(id);
        }

        public UserState GetByUser(string username)
        {
            return _unitOfWork.Query<UserState>().FirstOrDefault(x => x.Username == username);
        }

        public IEnumerable<UserState> GetAll()
        {
            return _unitOfWork.Query<UserState>();
        }

        public void Save(UserState userState)
        {
            _unitOfWork.Save(userState);
        }

        public void Update(UserState userState)
        {
            _unitOfWork.Update(userState);
        }

        public void Delete(UserState userState)
        {
            _unitOfWork.Delete(userState);
        }
    }
}