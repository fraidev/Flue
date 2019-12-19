namespace IdentityService.Domain.Command
{
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; } 
    }
    
    public class UserModel: LoginModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}