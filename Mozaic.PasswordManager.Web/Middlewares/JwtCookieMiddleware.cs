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
