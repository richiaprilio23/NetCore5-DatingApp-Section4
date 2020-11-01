using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController // panggil class BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public AccountController(DataContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        // public async Task<ActionResult<AppUser>> Register(string username, string password)
        {
            if (await UserExists(registerDto.Username)) 
            return BadRequest ("Username is Taken");

            using var hmac = new HMACSHA512(); //metode hashing
            var user = new AppUser
            {
                Username = registerDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync(); //save PasswordHash, PasswordSalt dan username

            // return user;
            return new UserDto
            {
                Username = user.Username,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto) //cek username dan password (loginDto entry user dan AppUser db)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Username == loginDto.Username);
            if (user == null) 
            return Unauthorized("Invalid Username");
            
            using var hmac = new HMACSHA512(user.PasswordSalt); //ubah PasswordSalt menjadi format HMACSHA512
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i=0; i < computedHash.Length; i++) //loop cek panjang computedHash dng PasswordHash
            {
                if (computedHash[i] != user.PasswordHash[i])
                return Unauthorized("Invalid Password");
            }

            // return user;
            return new UserDto
            {
                Username = user.Username,
                Token = _tokenService.CreateToken(user)
            };
        }

        private async Task<bool> UserExists(string username) //foreach username di dlm table (return true or false)
        {
            return await _context.Users.AnyAsync(x => x.Username == username.ToLower());
        }
    }
}