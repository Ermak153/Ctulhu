namespace Ctulhu.Models
{
    public class Users
    {
        public Users(string login, string email, string password, string role)
        {
            Users_Login = login;
            Users_Email = email;
            Users_Password = password;
            Users_Role = role;
        }

        public Users() { }

        public int Users_ID { get; set; }
        public string Users_Login { get; set; }
        public string Users_Email { get; set; }
        public string Users_Password { get; set; }
        public string Users_Role { get; set;}

        public override string ToString()
        {
            return $"Id:{Users_ID} Login:{Users_Login} Email:{Users_Email} Password:{Users_Password} Role:{Users_Role}";
        }
    }
}
