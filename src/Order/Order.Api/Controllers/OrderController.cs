using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Commands;
using Order.Application.Queries;
using Order.Application.Responses;

namespace Order.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController: ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OrderResponse>), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrdersByUsername(string userName)
        {
            var query = new GetOrdersByUserNameQuery(userName);
            var orders = await _mediator.Send(query);
            return Ok(orders);
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(OrderResponse), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<OrderResponse>> CheckoutOrder([FromBody] CheckoutOrderCommand checkoutOrderCommand)
        {
            var result = await _mediator.Send(checkoutOrderCommand);
            return Ok(result);
        }
        
    }
}