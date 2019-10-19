using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using ComputerScienceBlogBackEnd.DataAccess;
using ComputerScienceBlogBackEnd.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ComputerScienceBlogBackEnd.Services.UserManagement;

namespace ComputerScienceBlogBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]UserModel userParam)
        {
            var user = _userService.Authenticate(userParam.UserName, userParam.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        [Authorize/*(Roles = Role.Admin)*/]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetById(id);

            if (user == null)
            {
                return NotFound();
            }

            // only allow admins to access other user records
            var currentUserId = int.Parse(User.Identity.Name);
            //if (id != currentUserId && !User.IsInRole(Role.Admin))
            //{
            //    return Forbid();
            //}

            return Ok(user);
        }

        //private readonly UserService _userService;

        //public UsersController(UserService userService)
        //{
        //    _userService = userService;
        //}

        //[HttpGet]
        //public ActionResult<List<User>> Get() =>
        //    _userService.Get();

        //[HttpGet("{id:length(24)}", Name = "GetBook")]
        //public ActionResult<User> Get(string id)
        //{
        //    var user = _userService.Get(id);

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return user;
        //}

        //[HttpPost]
        //public ActionResult<User> Create(User user)
        //{
        //    _userService.Create(user);

        //    return CreatedAtRoute("GetBook", new { id = user.Id.ToString() }, user);
        //}

        //[HttpPut("{id:length(24)}")]
        //public IActionResult Update(string id, User userIn)
        //{
        //    var user = _userService.Get(id);

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    _userService.Update(id, userIn);

        //    return NoContent();
        //}

        //[HttpDelete("{id:length(24)}")]
        //public IActionResult Delete(string id)
        //{
        //    var user = _userService.Get(id);

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    _userService.Remove(user.Id);

        //    return NoContent();
        //}
    }
}