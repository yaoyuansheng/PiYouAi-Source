namespace Eason.Core.Users
{
    public class LoginResult
    {
        public long userId { get; set; }
        public string name { get; set; }
        public long roleId { get; set; }
        public string roleName { get; set; }
        public string message { get; set; }
        public string imgUrl { get; set; }
      
        public LoginResultType status { get; set; }
    }
}
