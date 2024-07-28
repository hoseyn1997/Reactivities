namespace API.DTOs
{
    /*this will have properties that we want send back 
    when user successfully logged in to the server*/
    public class UserDto
    {
        public string DisplayName { get; set; }
        public string Token { get; set; }
        public string Image { get; set; }
        public string UserName { get; set; }
    }
}