using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace payment_backend.src.Payment.BuildingBlocks.Infrastructure;

public static class ClaimsPrincipalExtensions {
    public static string UserId(this ClaimsPrincipal user) => user.Claims.First(i => i.Type == "userID").Value;
    public static string UserEmail(this ClaimsPrincipal user) => user.Claims.First(i => i.Type == "userEmail").Value;
    public static string UserRole(this ClaimsPrincipal user) => user.Claims.First(i => i.Type == "userRole").Value;
    public static string UserName(this ClaimsPrincipal user) => user.Claims.First(i => i.Type == "userName").Value;
}

public static class ControllerBaseExtensions {
    public static UserDTO GetUser(this ControllerBase controller) {
        return new UserDTO(controller.User.UserId(), controller.User.UserEmail(), controller.User.UserRole(), controller.User.UserName());  
    }
}