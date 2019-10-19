using ComputerScienceBlogBackEnd.DataAccess;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerScienceBlogBackEnd.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _users;

        public UserRepository(IComputerScienceBlogDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<User>(settings.UsersCollectionName);
        }

        public void Create(User user)
        {
            _users.InsertOne(user);
        }

        public void Update(string id, User userIn) =>
      _users.ReplaceOne(user => user.Id == id, userIn);

        public void Remove(User userIn) =>
            _users.DeleteOne(user => user.Id == userIn.Id);

        public void Remove(string id) =>
            _users.DeleteOne(user => user.Id == id);

        public IEnumerable<User> GetAll() => _users.Find(user => true).ToList();

        public User GetById(string id) =>
            _users.Find<User>(user => user.Id == id).FirstOrDefault();
    }
}
