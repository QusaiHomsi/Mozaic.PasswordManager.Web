//This middleware purpose is to read the cookie and take the jwt token from it
//and save it at the header so the [Authenticate] can read it
public class JwtCookieMiddleware
{
    private readonly RequestDelegate _next;

    public JwtCookieMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        
        if (context.Request.Cookies.TryGetValue("JWT", out string token))
        {
            
            context.Request.Headers.Add("Authorization", $"Bearer {token}");
        }

       
        await _next(context);
    }
}
