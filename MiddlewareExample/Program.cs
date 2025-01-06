using MiddlewareExample.Middlewares;

namespace MiddlewareExample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddTransient<MyCustomMiddleware>();

            var app = builder.Build();

            app.Use(async (HttpContext context, RequestDelegate next) =>
            {
                await context.Response.WriteAsync("Middleware 1 - Starts\n");
                await next(context);
                await context.Response.WriteAsync("Middleware 1 - Ends\n");
            });

            app.UseMyCustomMiddleware();

            app.UseWhenCustomeMiddleware();

            app.Use(async (HttpContext context, RequestDelegate next) =>
            {
                await context.Response.WriteAsync("Middleware 2 - Starts\n");
            });

            app.Run();
        }
    }
}
