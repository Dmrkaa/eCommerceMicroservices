using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Queries;
using Ordering.Application.Features.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ordering.Application.Features.Dtos;

namespace Ordering.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        public OrderController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("{userName}", Name = "GetOrder")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByUserName(string userName)
        {
            var query = new GetOrderListQuery(userName);
            var orders = _mediator.Send(query);
            return Ok(orders);
        }

    }
}
