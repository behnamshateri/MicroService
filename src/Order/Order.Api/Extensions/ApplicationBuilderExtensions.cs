using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Order.Api.RabbitMq;

namespace Order.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        private static EventBusRabbitMqConsumer Listener { get; set; }
        public static IApplicationBuilder UseRabbitMqListener(this IApplicationBuilder app)
        {
            Listener = app.ApplicationServices.GetService<EventBusRabbitMqConsumer>();
            IHostApplicationLifetime life = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            if (life != null)
            {
                life.ApplicationStarted.Register(OnStarted);
                life.ApplicationStopping.Register(OnStopping);
            }
            
            return app;
        }

        private static void OnStarted()
        {
            Console.WriteLine("Application is started");
            Listener.Consume();
        }
        
        private static void OnStopping()
        {
            Listener.Disconnect();
        }
    }
}