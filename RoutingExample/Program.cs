using System.ComponentModel;
using RoutingExample.CustomeConstraints;

namespace RoutingExample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddRouting(options =>
            {
                options.ConstraintMap.Add("months", typeof(MonthsRouteConstraints));
            });
            
            var app = builder.Build();

            app.Use(async (context, next) =>
            {
                Microsoft.AspNetCore.Http.Endpoint? endPoint = context.GetEndpoint();
                if (endPoint != null)
                {
                    await context.Response.WriteAsync($"Start - Endpoint: {endPoint.DisplayName}\n");
                }   
                await next(context);
                if (endPoint != null)
                {
                    await context.Response.WriteAsync($"End - Endpoint: {endPoint.DisplayName}\n");
                }
            });

            app.MapGet("/", async (context) => {
                await context.Response.WriteAsync("---------\n");
                await context.Response.WriteAsync("Welcome to RoutingExample project\n");
                await context.Response.WriteAsync("---------\n");
            });

            // Ta có thể sử dụng MapGroup() thay thể (ASP.NET Core 6.0+)
            app.Map("/examples", supApp =>
            {
                supApp.UseRouting();

                supApp.UseEndpoints(endpoints =>
                {
                    endpoints.MapGet("/map", async context =>
                    {
                        await context.Response.WriteAsync("---------\n");
                        await context.Response.WriteAsync("In \"/examples/map\" - GET method\n");
                        await context.Response.WriteAsync("---------\n");
                    });

                    endpoints.MapPost("/map", async context =>
                    {
                        await context.Response.WriteAsync("---------\n");
                        await context.Response.WriteAsync("In \"/examples/map\" - POST method\n");
                        await context.Response.WriteAsync("---------\n");
                    });

                    endpoints.MapPut("/map", async context =>
                    {
                        await context.Response.WriteAsync("---------\n");
                        await context.Response.WriteAsync("In \"/examples/map\" - PUT method\n");
                        await context.Response.WriteAsync("---------\n");

                    });

                    endpoints.MapPatch("/map", async context =>
                    {
                        await context.Response.WriteAsync("---------\n");
                        await context.Response.WriteAsync("In \"/examples/map\" - PATCH method\n");
                        await context.Response.WriteAsync("---------\n");
                    });

                    endpoints.MapDelete("/map", async context =>
                    {
                        await context.Response.WriteAsync("---------\n");
                        await context.Response.WriteAsync("In \"/examples/map\" - DELETE method\n");
                        await context.Response.WriteAsync("---------\n");
                    });
                });
            });

            app.Map("/files/{filename}.{extension}", async (string filename, string extension, HttpContext context) =>
            {
                await context.Response.WriteAsync("---------\n");
                await context.Response.WriteAsync($"In files\n");
                await context.Response.WriteAsync($"Files name: {filename}\n");
                await context.Response.WriteAsync($"Extension name: {extension}\n");
                await context.Response.WriteAsync("---------\n");
            });

            app.Map("/users/profile/{userId:int=123}", async (string userId, HttpContext context) =>
            {
                await context.Response.WriteAsync("---------\n");
                await context.Response.WriteAsync($"In user profile\n");
                await context.Response.WriteAsync($"userId: {userId}\n");
                await context.Response.WriteAsync("---------\n");
            });

            app.Map("/examples/parameters/{optional?}", async (string? optional, HttpContext context) =>
            {
                await context.Response.WriteAsync("---------\n");
                await context.Response.WriteAsync($"In example for optional parameter\n");
                if(optional != null)
                {
                    await context.Response.WriteAsync($"Optional is chosen\n");
                } else
                {
                    await context.Response.WriteAsync($"Optional is not chosen\n");

                }
                await context.Response.WriteAsync("---------\n");
            });

            app.Map("/daily-digest-report/{reportDate:datetime}", async (string reportDate, HttpContext context) =>
            {
                await context.Response.WriteAsync("---------\n");
                DateTime reportDateDT = Convert.ToDateTime(reportDate);
                await context.Response.WriteAsync("In daily digest report\n");
                await context.Response.WriteAsync($"Report date: {reportDateDT}\n");
                await context.Response.WriteAsync("---------\n");
            });

            app.Map("/cities/{cityid:guid}", async (Guid cityid, HttpContext context) =>
            {
                await context.Response.WriteAsync("---------\n");
                await context.Response.WriteAsync("In /cities route\n");
                await context.Response.WriteAsync($"Report date: {cityid}\n");
                await context.Response.WriteAsync("---------\n");
            });

            app.Map("/sales-report-v1/{year:int:min(1990)}/{month:regex(^(apr|jul|oct|jan)$)}", async (int year, string month, HttpContext context) =>
            {
                await context.Response.WriteAsync("---------\n");
                await context.Response.WriteAsync("In \"/sales-report\" route\n");
                await context.Response.WriteAsync($"Year: {year}\n");
                await context.Response.WriteAsync($"Month: {month}\n");
                await context.Response.WriteAsync("---------\n");
            });

            app.Map("/sales-report-v2/{year:int:min(1990)}/{month:months}", async (int year, string month, HttpContext context) =>
            {
                await context.Response.WriteAsync("---------\n");
                await context.Response.WriteAsync("In \"/sales-report-v2\" route\n");
                await context.Response.WriteAsync($"Year: {year}\n");
                await context.Response.WriteAsync($"Month: {month}\n");
                await context.Response.WriteAsync("---------\n");
            });

            app.Use(async (context, next) =>
            {
                await next(context);

                if (!context.Response.HasStarted)
                {
                    context.Response.StatusCode = 404;
                    await context.Response.WriteAsync("Not found");
                }
            });

            app.Run();
        }
    }
}
