using ComputerScienceBlogBackEnd.DataAccess;
using System.Collections.Generic;

namespace ComputerScienceBlogBackEnd.Services.UserManagement
{
    public interface IUserService
    {
        User Authenticate(string username, string password);

        IEnumerable<User> GetAll();

        User GetById(string id);

        void Update(string id, User userIn);

        void Create(User user);

        void Remove(string id);
    }
}
