namespace API.middlewares;

public class ExceptionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            Console.WriteLine($"from my middleware : {e}");
            throw;
        }
    }
}