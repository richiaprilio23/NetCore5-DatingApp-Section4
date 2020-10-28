using System.Collections.Generic;
using System.Linq;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;
 
        public UsersController(DataContext Context)
        {
            _context = Context;
        }

        //api/users
        [HttpGet]
        public ActionResult<IEnumerable<AppUser>> GetUsers()
        //IEnumerable = untuk nilai kembalian berupa list di dalam AppUser
        {
            var users = _context.Users.ToList();
            return users;
        }

        //api/users/3
        [HttpGet("{id}")]
        public ActionResult<AppUser> GetUser(int id)
        //IEnumerable = tidak dipakai karena nilai kembalian hanya sebuah data dari parameter id
        {
            return _context.Users.Find(id);
            
        }
    }
}