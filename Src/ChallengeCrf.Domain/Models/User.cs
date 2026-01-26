namespace ChallengeCrf.Domain.Models;

public class User
{
    public int UserId { get; set; }
    public string EmailAddress { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

}