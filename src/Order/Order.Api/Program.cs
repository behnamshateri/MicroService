using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Console;
using Order.Infrastructure.Data;

namespace Order.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();
            CreateAndSeedDatabase(host);
            host.Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });


        private static void CreateAndSeedDatabase(IHost host)
        {
            // Create scope from host to get service provider
            using var scope = host.Services.CreateScope();
            
            IServiceProvider services = scope.ServiceProvider;
                
            // create logger factory
            ILoggerFactory loggerFactory = new LoggerFactory();
            // ILogger<Program> loggerFactory = services.GetRequiredService<ILogger<Program>>();
            
            try
            {
                OrderContext orderContext = services.GetRequiredService<OrderContext>();
                OrderContextSeed.SeedAsync(orderContext, loggerFactory);
            }
            catch (Exception e)
            {
                ILogger<Program> logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(e.Message);
            }
        }
    }
}
