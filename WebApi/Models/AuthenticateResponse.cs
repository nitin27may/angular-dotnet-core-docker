namespace WebApi.Models
{
    public class AuthenticateResponse
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public bool Authenticate { get; set; }


        public AuthenticateResponse(User user, string token, bool authenticate)
        {
            Id = (long)(user?.Id);
            FirstName = user?.FirstName;
            LastName = user?.LastName;
            Username = user?.Username;
            Token = token;
            Authenticate = authenticate;
        }
    }
}
