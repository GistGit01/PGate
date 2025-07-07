namespace WebAPI.Models.Portal
{
    public class SigninRequest :IRequestModel
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }

    public class SigninResult : ResultModel
    {
        public string Token { get; set; }
    }
}
