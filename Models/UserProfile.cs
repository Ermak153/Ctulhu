namespace Ctulhu.Models
{
    public class UserProfile
    {
        public int ID { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string OldPassword { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
