using System.Security.Claims;

public static class TestClaimsPrincipalFactory
{
    public static ClaimsPrincipal Create(string userId)
    {
        var claims = new List<Claim>
        {
            new Claim("userID", userId),             // required by UserId()
            new Claim("userEmail", "test@example.com"), // required by UserEmail()
            new Claim("userRole", "User"),           // required by UserRole()
            new Claim("userName", "Test User")       // required by UserName()
        };

        var identity = new ClaimsIdentity(claims, "TestAuthType");
        return new ClaimsPrincipal(identity);
    }
}