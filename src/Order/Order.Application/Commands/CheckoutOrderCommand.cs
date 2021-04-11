using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mapster;
using MediatR;
using Order.Application.Queries;
using Order.Application.Responses;
using Order.Core.Repositories;

namespace Order.Application.Commands
{
    public class CheckoutOrderCommand : IRequest<OrderResponse>
    {
        public string UserName { get; set; }
        public string TotalPrice { get; set; }

        // Billing Address
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string AddressLine { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        // Payment
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CVV { get; set; }
        public int PaymentMethod { get; set; }
    }
}