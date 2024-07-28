namespace API.DTOs
{
    /*It will be used when a user logges in
    we will get these from user*/
    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}