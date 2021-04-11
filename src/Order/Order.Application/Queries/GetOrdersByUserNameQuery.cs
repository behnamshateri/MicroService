using System.Collections.Generic;
using MediatR;
using Order.Application.Responses;

namespace Order.Application.Queries
{
    public class GetOrdersByUserNameQuery : IRequest<IEnumerable<OrderResponse>>
    {
        public string UserName { get; }

        public GetOrdersByUserNameQuery(string userName)
        {
            UserName = userName;
        }
    }
}