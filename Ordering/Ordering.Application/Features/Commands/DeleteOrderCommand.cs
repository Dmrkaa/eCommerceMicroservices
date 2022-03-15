using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
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
    public class DeleteOrderCommand : IRequest<Unit>
    {
        public int Id { get; set; }
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

        public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, Unit>
        {
            IOrderRepository _orderRepository;
            IMapper _mapper;
            ILogger<DeleteOrderCommand> _logger;
            public DeleteOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, ILogger<DeleteOrderCommand> logger)
            {
                _orderRepository = orderRepository;
                _mapper = mapper;
                _logger = logger;
            }
            public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
            {
                var orderToDelete = _mapper.Map<Order>(_orderRepository.GetOrdersByUserName(request.UserName));

                if (orderToDelete == null)
                {
                    //throw new NotFoundException(nameof(Order), request.Id);
                }
                await _orderRepository.DeleteAsync(orderToDelete);
                _logger.LogInformation($"Order {orderToDelete.Id} is deleted.");
                return Unit.Value;
            }
        }
    }
}
