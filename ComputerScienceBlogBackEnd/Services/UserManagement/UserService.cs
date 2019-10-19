﻿using ComputerScienceBlogBackEnd.Repositories;
using System;
using System.Collections.Generic;

namespace ComputerScienceBlogBackEnd.Services.UserManagement
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository userRepository)
        {
            _repository = userRepository;
        }

        public UserModel Authenticate(string username, string password)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public UserModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        //private readonly IMongoCollection<User> _users;

        //public UserService(IComputerScienceBlogDatabaseSettings settings)
        //{
        //    var client = new MongoClient(settings.ConnectionString);
        //    var database = client.GetDatabase(settings.DatabaseName);

        //    _users = database.GetCollection<User>(settings.UsersCollectionName);
        //}

        //public List<User> Get() => _users.Find(user => true).ToList();

        //public User Get(string id) =>
        //    _users.Find<User>(user => user.Id == id).FirstOrDefault();

        //public User Create(User user)
        //{
        //    _users.InsertOne(user);
        //    return user;
        //}

        //public void Update(string id, User userIn) =>
        //    _users.ReplaceOne(user => user.Id == id, userIn);

        //public void Remove(User userIn) =>
        //    _users.DeleteOne(user => user.Id == userIn.Id);

        //public void Remove(string id) =>
        //    _users.DeleteOne(user => user.Id == id);
    }
}
