using System.IO;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace WingsOn.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                       .ConfigureServices(x => x.AddAutofac())
                       .UseKestrel()
                       .UseContentRoot(Directory.GetCurrentDirectory())
                       .UseStartup<Startup>()
                       .UseSerilog()
                       .Build();
            host.Run();
        }
    }
}
