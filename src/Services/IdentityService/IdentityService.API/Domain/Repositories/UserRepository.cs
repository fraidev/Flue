using System;
using System.Collections.Generic;
using System.Linq;
using IdentityService.Domain.State;
using IdentityService.Infrastructure.Persistence;

namespace IdentityService.Domain.Repositories
{
    public interface IUserRepository
    {
        User GetById(Guid id);
        User GetByUsername(string id);
        IEnumerable<User> GetAll();
        void Save(User user);
        void Update(User user);
        void Delete(User user);
    }
    public class UserRepository: IUserRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public User GetById(Guid id)
        {
            return _unitOfWork.GetById<User>(id);
        }

        public User GetByUsername(string username)
        {
            return _unitOfWork.Query<User>().FirstOrDefault(x => x.Username == username);
        }

        public IEnumerable<User> GetAll()
        {
            return _unitOfWork.Query<User>();
        }
        
        public void Save(User user)
        {
            _unitOfWork.Save(user);
        }

        public void Update(User user)
        {
            _unitOfWork.Update(user);
        }

        public void Delete(User user)
        {
            _unitOfWork.Delete(user);
        }
    }
}