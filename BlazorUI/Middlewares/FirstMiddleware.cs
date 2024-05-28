namespace BlazorUI.Components.Middlewares;

public class FirstMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.WebSockets.IsWebSocketRequest)
            Console.WriteLine("Create Web Socket");
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        // if (context.WebSockets.IsWebSocketRequest)
        //     Console.WriteLine("Close Web Socket");
    }
}