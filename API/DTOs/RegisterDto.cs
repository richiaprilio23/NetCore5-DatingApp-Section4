using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDto
    {   
        [Required] //validasi error if empty
        public string Username {get;set;}

        [Required] //validasi error if empty
        public string Password {get;set;}
    }
}