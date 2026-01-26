using ChallengeCrf.Domain.Models;
using ChallengeCrf.Infra.Data.Context;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace ChallengeCrf.Api.Controllers;

public class UserMap
{
    private UserContext _context;
    public UserMap(UserContext context)
    {
        _context = context;
    }
    public static void ExposeMaps(WebApplication app)
    {
        app.MapPost("api/loginuser/", async ([FromBody] User user, UserContext context,
            IHttpContextAccessor httpContextAccessor, ILogger<Program> logger) =>
        {
            try
            {
                User loggedInUser = context.Users.Where(u => u.EmailAddress == user.EmailAddress && u.Password == user.Password)
                .FirstOrDefault();

                if (loggedInUser != null)
                {
                    //create a clain
                    var claim = new Claim(ClaimTypes.Name, loggedInUser.EmailAddress);
                    //create caimsIdenttity
                    var claimsIdentity = new ClaimsIdentity(new[] { claim }, "serverAuth");
                    //create claimsPrincipal
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    //Sign In User
                    await httpContextAccessor.HttpContext.SignInAsync(claimsPrincipal);

                }

                return Results.Ok(loggedInUser);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{ex.Message}");
                return Results.BadRequest(ex);
            }

            return Results.Ok();
        });

        app.MapGet("api/currentuser/", async ([FromBody] User user, UserContext context,
            IHttpContextAccessor httpContextAccessor, ILogger<Program> logger) =>
        {
            User currentUser = new User();

            if (httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                currentUser.EmailAddress = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            }

            return Results.Ok(currentUser);
        });

        app.MapGet("api/logoutuser/", async (IHttpContextAccessor httpContextAccessor, ILogger<Program> logger) =>
        {
            await httpContextAccessor.HttpContext.SignOutAsync();

            return Results.Ok();
        });
    }
}
