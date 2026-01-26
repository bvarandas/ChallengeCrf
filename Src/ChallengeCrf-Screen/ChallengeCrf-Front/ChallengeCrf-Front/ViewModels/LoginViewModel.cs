using ChallengeCrf_Front.Data;

namespace ChallengeCrf_Front.ViewModels;

public class LoginViewModel
{
    private readonly HttpClient _httpClient;
    public string EmailAddress { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public LoginViewModel()
    {
    }

    public LoginViewModel(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task LoginUser()
    {
        await _httpClient.PostAsJsonAsync<User>("loginuser", this);
    }

    public static implicit operator LoginViewModel(User user)
    {
        return new LoginViewModel
        {
            EmailAddress = user.EmailAddress,
            Password = user.Password
        };
    }

    public static implicit operator User(LoginViewModel vm)
    {
        return new User
        {
            EmailAddress = vm.EmailAddress,
            Password = vm.Password
        };
    }
}
