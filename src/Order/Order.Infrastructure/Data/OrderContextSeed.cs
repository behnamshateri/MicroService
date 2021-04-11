using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Order.Infrastructure.Data;
using Order = Basket.Entities.Order;

namespace Infrastructure.Data
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext, ILoggerFactory loggerFactory, int retry = 0)
        {
            int retryForAvailability = retry;

            try
            {
                await orderContext.Database.MigrateAsync();  // migration ro anjam mide

                if (!await orderContext.Orders.AnyAsync()) // agar dadeyi nabud yeseri pishfarz ezafe mikonim
                {
                    await orderContext.Orders.AddRangeAsync(GetPreConfiguredOrders());
                    await orderContext.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                if (retryForAvailability < 5) // try it 5 times
                {
                    retryForAvailability++;
                    var log = loggerFactory.CreateLogger<OrderContextSeed>();
                    log.LogError(e.Message);

                    System.Threading.Thread.Sleep(2000);
                    
                    await SeedAsync(orderContext, loggerFactory, retryForAvailability);
                }
                else
                {
                    throw;
                }
            }
        }


        private static IEnumerable<Basket.Entities.Order> GetPreConfiguredOrders()
        {
            return new List<Basket.Entities.Order>()
            {
                new Basket.Entities.Order()
                {
                    UserName = "behnamshateri",
                    FirstName = "Behnam",
                    LastName = "Shateri",
                    EmailAddress = "behnamshateri@yahoo.com",
                    AddressLine = "test address",
                    Country = "iran"
                }
            };
        }
    }
}