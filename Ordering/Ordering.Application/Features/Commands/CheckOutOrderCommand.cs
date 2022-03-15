using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Commands
{
    public class CheckOutOrderCommand : IRequest<int>
    {
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }

        // BillingAddress
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

        public class CheckOutOrderCommandHandler : IRequestHandler<CheckOutOrderCommand, int>
        {
            private readonly IOrderRepository _orderRepository;
            private readonly IMapper _mapper;
            private readonly INotificationService _notificationService;
            private readonly ILogger<CheckOutOrderCommand> _logger;
            public CheckOutOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, INotificationService notificationService, ILogger<CheckOutOrderCommand> logger)
            {
                _orderRepository = orderRepository;
                _mapper = mapper;
                _notificationService = notificationService;
                _logger = logger;
            }
            public async Task<int> Handle(CheckOutOrderCommand request, CancellationToken cancellationToken)
            {
                var orderEntity = _mapper.Map<Order>(request);
                var order = await _orderRepository.AddAsync(orderEntity);
                _logger.LogInformation($"Order { order.Id} is created.");
                //await SendNotification(order);
                return order.Id;
            }

            private async Task SendNotification(Order order)
            {

            }
        }

    }
}
