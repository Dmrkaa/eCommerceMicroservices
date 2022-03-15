using CartAPI.Entities;
using CartAPI.Services.BasketRepositories.Interfaces;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CartAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : ControllerBase
    {

        private readonly ILogger<CartController> _logger;
        private readonly ICartRepository _cartRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        //private readonly IMapper _mapper;

        public CartController(ILogger<CartController> logger, ICartRepository repository, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _cartRepository = repository ?? throw new ArgumentNullException(nameof(repository));
            _publishEndpoint = publishEndpoint;
        }

        [Route("action")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] CartCheckout checkout)
        {
            // get existing basket with total price 
            // Set TotalPrice on basketCheckout eventMessage
            // send checkout event to rabbitmq
            // remove the basket
            // get existing basket with total price
            var basket = await _cartRepository.GetBasket(checkout.UserName);
            if (basket == null)
            {
                return BadRequest();
            }
            // send checkout event to rabbitmq
            var eventMessage = _mapper.Map<CartCheckoutEvent>(checkout);
            await _publishEndpoint.Publish(eventMessage);
            await _cartRepository.DeleteBasket(basket.UserName);
            return Accepted();
        }

        [HttpGet("{userName}", Name = "GetBasket")]
        [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Cart>> GetBasket(string userName)
        {
            var basket = await _cartRepository.GetBasket(userName);
            return Ok(basket ?? new Cart(userName));
        }
        [HttpPost]
        [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Cart>> UpdateBasket([FromBody] Cart basket)
        {
            return Ok(await _cartRepository.UpdateBasket(basket));
        }

        [HttpDelete("{userName}", Name = "DeleteBasket")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            await _cartRepository.DeleteBasket(userName);
            return Ok();
        }
    }
}

