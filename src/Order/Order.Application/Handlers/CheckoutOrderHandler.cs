using System;
using System.Threading;
using System.Threading.Tasks;
using Mapster;
using MediatR;
using Order.Application.Commands;
using Order.Application.Responses;
using Order.Core.Repositories;

namespace Order.Application.Handlers
{
    public class CheckoutOrderHandler : IRequestHandler<CheckoutOrderCommand, OrderResponse>
    {
        private readonly IOrderRepository _orderRepository;

        public CheckoutOrderHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository; 
        }

        
        public async Task<OrderResponse> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            // map to response
            Basket.Entities.Order order = request.Adapt<Basket.Entities.Order>();

            if (order == null)
            {
                throw new ApplicationException("not mapped");
            }
            
            var newOrder = await _orderRepository.AddAsync(order);
            
            OrderResponse orderResponse = newOrder.Adapt<OrderResponse>();

            return orderResponse;
        }
    }
}