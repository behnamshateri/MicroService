using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mapster;
using MediatR;
using Order.Application.Queries;
using Order.Application.Responses;
using Order.Core.Repositories;

namespace Order.Application.Handlers
{
    public class GetOrdersByUserNameHandler : IRequestHandler<GetOrdersByUserNameQuery, IEnumerable<OrderResponse>>
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrdersByUserNameHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository; 
        }

        public async Task<IEnumerable<OrderResponse>> Handle(GetOrdersByUserNameQuery request, CancellationToken cancellationToken)
        {
            // get orders list
            var orderList = await _orderRepository.GetOrderByUserName(request.UserName);
            
            // map to response
            var orderResponseList = orderList.Adapt<IEnumerable<OrderResponse>>();

            return orderResponseList;
        }
    }
}