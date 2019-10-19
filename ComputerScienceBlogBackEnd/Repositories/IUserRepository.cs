using ComputerScienceBlogBackEnd.DataAccess;
using System.Collections.Generic;

namespace ComputerScienceBlogBackEnd.Repositories
{
    public interface IUserRepository
    {
        void Create(User user);
        void Update(string id, User userIn);
        void Remove(User userIn);
        void Remove(string id);
        List<User> GetAll();
        User GetById(string id);
    }
}
