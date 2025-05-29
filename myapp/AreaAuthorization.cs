using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

public class AreaAuthorization : ActionFilterAttribute
{
    private readonly string _requiredUserType;

    public AreaAuthorization(string requiredUserType)
    {
        _requiredUserType = requiredUserType;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var token = context.HttpContext.Request.Cookies["AuthToken"];

        if (string.IsNullOrEmpty(token))
        {
            context.Result = new RedirectToActionResult("Login", "Home", null);
            return;
        }

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

        var userType = jwtToken?.Claims.FirstOrDefault(c => c.Type == "UserType")?.Value;

        if (userType == null || userType != _requiredUserType)
        {
            context.Result = new RedirectToActionResult("Login", "Home", null);
        }
    }
}
