using ComputerScienceBlogBackEnd.DataAccess;
using ComputerScienceBlogBackEnd.Helpers;
using ComputerScienceBlogBackEnd.Infrastructure.Exceptions;
using ComputerScienceBlogBackEnd.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace ComputerScienceBlogBackEnd.Services.UserManagement
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings, IUserRepository userRepository)
        {
            _appSettings = appSettings.Value;
            _repository = userRepository;
        }

        public User Authenticate(string userName, string password)
        {
            var user = _repository.GetAll().SingleOrDefault(x => x.UserName == userName && x.Password == password);

            if (user == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            user.Password = null;

            return user;
        }

        public void Create(User user)
        {
            var dbUsers = _repository.GetAll().Where(u => u.UserName == user.UserName || u.Email == user.Email).ToArray();
            if(dbUsers.Length > 0)
            {
                throw new RequestedResourceHasConflictException("User with that username or mail already exists");
            }

            _repository.Create(user);
        }

        public void Remove(string id)
        {
            _repository.Remove(id);
        }

        public IEnumerable<User> GetAll()
        {
            return _repository.GetAll().Select(x => {
                x.Password = null;
                return x;
            });
        }

        public User GetById(string id)
        {
            var user = _repository.GetById(id);

            if (user != null)
                user.Password = null;

            return user;
        }

        public void Update(string id, User userIn)
        {
            _repository.Update(id, userIn);
        }
    }
}
