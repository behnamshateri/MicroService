using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using Basket.Entities;
using Basket.Repositories.Interfaces;
using EventBusRabbitMQ.Common;
using EventBusRabbitMQ.Events;
using EventBusRabbitMQ.Producer;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DefaultNamespace
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly EventBusRabbitMqProducer _eventBus;

        public BasketController(IBasketRepository basketRepository, EventBusRabbitMqProducer eventBus)
        {
            _basketRepository = basketRepository;    
            _eventBus = eventBus;    
        }

        [HttpGet]
        [ProducesResponseType(typeof(BasketCart), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<BasketCart>> GetBasket(string userName)
        {
             var result = await _basketRepository.GetBasket(userName);
             return Ok(result ?? new BasketCart(userName));
        }
        
        
        [HttpPost]
        [ProducesResponseType(typeof(BasketCart), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<BasketCart>> UpdateBasket([FromBody] BasketCart basketCart)
        {
            return Ok(await _basketRepository.UpdateBasket(basketCart));
        }
        
        
        [HttpDelete("{userName}")]
        [ProducesResponseType(typeof(BasketCart), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<BasketCart>> DeleteBasket(string userName)
        {
            return Ok(await _basketRepository.DeleteBasket(userName));
        }
        
        [HttpPost("[action]")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<ActionResult<BasketCart>> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            try
            {
                // get basket
                BasketCart basketCart = await _basketRepository.GetBasket(basketCheckout.UserName);

                if (basketCart == null)
                {
                    return BadRequest();
                }

                // remove basket
                // bool result = await _basketRepository.DeleteBasket(basketCheckout.UserName);
                //
                // if (result == false)
                // {
                //     return BadRequest();
                // }

                BasketCheckoutEvent basketCheckoutEvent = basketCheckout.Adapt<BasketCheckoutEvent>();
                basketCheckoutEvent.RequestId = Guid.NewGuid();
                basketCheckoutEvent.TotalPrice = basketCart.TotalPrice.ToString(CultureInfo.InvariantCulture);

                try
                {
                    _eventBus.PublishBasketCheckout(EventBusConstant.BasketCheckoutQueue, basketCheckoutEvent);
                }
                catch (Exception)
                {
                    throw;
                }


                return Accepted();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        
        
    }
}