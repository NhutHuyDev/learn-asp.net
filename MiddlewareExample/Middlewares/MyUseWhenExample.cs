
namespace MiddlewareExample.Middlewares
{
    public static class UseWhenMiddlewareExtension
    {
        public static IApplicationBuilder UseWhenCustomeMiddleware(this IApplicationBuilder app)
        {
            return app.UseWhen(
                context => context.Request.Query.ContainsKey("userId"),
                app =>
                {
                    app.Use(async (context, next) =>
                    {
                        await context.Response.WriteAsync($"Hello User {context.Request.Query["userId"]} from UseWhenCustomeMiddleware \n");
                        await next(context);
                    });
                });
        }
    }
}
