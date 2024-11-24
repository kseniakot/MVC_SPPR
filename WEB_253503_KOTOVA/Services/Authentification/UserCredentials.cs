namespace WEB_253503_KOTOVA.UI.Services.Authentification
{
    public class UserCredentials
    {
        public string Type { get; set; } = "Password";
        public bool Temporary { get; set; } = false;
        public string Value { get; set; }
    }
}
