using Microsoft.Extensions.FileProviders;

namespace StaticFilesExample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions()
            {
                WebRootPath = "public"
            });

            var app = builder.Build();

            app.UseStaticFiles(); // work with root

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "secondpublic"))
            }); // work with "secondpublic"

            app.MapGet("/", () => "Welcome to StaticFileExample project");

            app.Run();
        }
    }   
}
