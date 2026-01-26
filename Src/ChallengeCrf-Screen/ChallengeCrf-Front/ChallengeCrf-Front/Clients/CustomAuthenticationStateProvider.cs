using ChallengeCrf_Front.Data;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace ChallengeCrf_Front.Clients;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;

    public CustomAuthenticationStateProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        User currentUser = await _httpClient.GetFromJsonAsync<User>("user/currentuser/") ?? throw new Exception("Could not find User!");

        if (currentUser != null && currentUser.EmailAddress != null)
        {
            //create a clain
            var claim = new Claim(ClaimTypes.Name, currentUser.EmailAddress);
            //create caimsIdenttity
            var claimsIdentity = new ClaimsIdentity(new[] { claim }, "serverAuth");
            //create claimsPrincipal
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            return new AuthenticationState(claimsPrincipal);

        }
        else
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

    }
}
