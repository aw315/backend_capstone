using System.ComponentModel.DataAnnotations;


namespace backend_capstone.Models
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }

    }
}
