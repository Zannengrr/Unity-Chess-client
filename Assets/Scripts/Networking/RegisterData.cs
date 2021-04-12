public class RegisterData
{
    public string Email;
    public string Username;
    public string Password;

    public RegisterData() { }
    public RegisterData(string _email, string _username, string _password)
    {
        Email = _email;
        Username = _username;
        Password = _password;
    }
}
