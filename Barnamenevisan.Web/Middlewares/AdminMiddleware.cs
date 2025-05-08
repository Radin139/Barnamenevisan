using Barnamenevisan.Core.Extensions;

namespace Barnamenevisan.Web.Middlewares;

public class AdminMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        
        if (context.Request.Path.ToString().ToLower().StartsWith("/admin"))
        {
            if (!context.User.GetIsAdmin())
            {
                context.Response.Redirect("/login");
            }
        }
        await next(context);
    }
}