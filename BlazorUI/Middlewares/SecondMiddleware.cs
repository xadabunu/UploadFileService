namespace BlazorUI.Components.Middlewares;

public class SecondMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        Task.Delay(1000);
        throw new ArithmeticException("From SecondMiddleware");
        await next(context);
    }
}