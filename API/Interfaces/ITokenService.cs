using API.Entities;

namespace API.Interfaces
{
    public interface ITokenService //implementasi dari kelas2 manapun dan hanya berisi signature
    {
        string CreateToken(AppUser user);   
    }
}