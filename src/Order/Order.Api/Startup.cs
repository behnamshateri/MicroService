using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using EventBusRabbitMQ;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Order.Api.Extensions;
using Order.Api.RabbitMq;
using Order.Application.Handlers;
using Order.Application.Responses;
using Order.Core.Repositories;
using Order.Core.Repositories.Base;
using Order.Infrastructure.Data;
using Order.Infrastructure.Repositories;
using Order.Infrastructure.Repositories.Base;
using RabbitMQ.Client;

namespace Order.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<OrderContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("OrderConnection")), ServiceLifetime.Singleton);

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IOrderRepository), typeof(OrderRepository));
            services.AddTransient<IOrderRepository, OrderRepository>();
            
            // register mediatR
            services.AddMediatR(typeof(CheckoutOrderHandler).Assembly);

            services.AddSingleton<IRabbitMqConnection>(sp =>
            {
                var factory = new ConnectionFactory()
                {
                    HostName = Configuration["EventBus:HostName"],
                    Port = Convert.ToInt16(Configuration["EventBus:Port"])
                };

                if (!string.IsNullOrEmpty(Configuration["EventBus:UserName"]))
                {
                    factory.UserName = Configuration["EventBus:UserName"];
                }

                if (!string.IsNullOrEmpty(Configuration["EventBus:Password"]))
                {
                    factory.Password = Configuration["EventBus:Password"];
                }
                
                Console.WriteLine(factory);
                Console.WriteLine(Configuration["EventBus:HostName"]);
                Console.WriteLine(Configuration["EventBus:Port"]);
                Console.WriteLine(Configuration["EventBus:UserName"]);
                Console.WriteLine(Configuration["EventBus:Password"]);

                return new RabbitMqConnection(factory);
            });
            
            services.AddSingleton<EventBusRabbitMqConsumer>();

            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo()
            {
                Title = "Order API",
                Version = "V1"
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseRabbitMqListener();

            app.UseSwagger();
            app.UseSwaggerUI(c => 
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order APi v1")
            );
        }
    }
}
