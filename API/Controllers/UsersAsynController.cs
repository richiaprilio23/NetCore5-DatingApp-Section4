using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersAsynController : ControllerBase
    {
        //API metode asynchronous multithread digunakan saat proses memanipulasi data (filter, sortir, dsb) melebihi 1 jt proses data, 
        // dengan ribuan user akses 

        private readonly DataContext _context;
 
        public UsersAsynController(DataContext Context)
        {
            _context = Context;
        }

        //api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            //saat user akses masuk ke db, kode ini berhenti sebentar, menunggu
            //ditangguhakan, kemudian masuk dan eksekusi query ke db
            return await _context.Users.ToListAsync();

        }

        //api/users/3
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            return await _context.Users.FindAsync(id);
            
        }
    }
}