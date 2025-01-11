using Microsoft.Extensions.Primitives;

namespace HttpExample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            app.Use(async (HttpContext context, Func<Task> next) =>
            {
                context.Response.Headers.ContentType = "text/html";
                if (context.Request.Method == "GET")
                {
                    if (context.Request.Headers.ContainsKey("User-Agent"))
                    {
                        string? userAgentId = context.Request.Headers.UserAgent;
                        await context.Response.WriteAsync(userAgentId ?? string.Empty);
                    }
                    else
                    {
                        await context.Response.WriteAsync("Hello World");
                    }
                } else if (context.Request.Method == "POST")
                {
                    // Handle x-www-form-urlencoded
                    StreamReader reader = new(context.Request.Body);
                    string body = await reader.ReadToEndAsync();

                    Dictionary<string, StringValues> queryDict =
                        Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(body);
                        
                    foreach(var item in queryDict) {
                        if (!StringValues.IsNullOrEmpty(item.Value))
                        {
                            await context.Response.WriteAsync($"{item.Key}: {string.Join(", ", [.. item.Value])}\n");

                        }
                    }
                }
            });

            app.Run();
        }
    }
}
