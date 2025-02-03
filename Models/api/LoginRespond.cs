namespace Institute_Management.Models.api
{
    public class LoginRespond
    {
        public string UserName { get; set; }
        public string AccessToken { get; set; }
        public int ExpiresIn { get; set; }
    }
}
