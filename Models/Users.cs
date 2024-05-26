namespace Ctulhu.Models
{
    public class Users
    {
        public Users() { }
        public Users(string login, string email, string password, string role)
        {
            Login = login;
            Email = email;
            Password = password;
            Role = role;
        }

        public int ID { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public override string ToString()
        {
            return $"Id:{ID} Login:{Login} Email: {Email} Password:{Password} Role:{Role}";
        }
    }
}
