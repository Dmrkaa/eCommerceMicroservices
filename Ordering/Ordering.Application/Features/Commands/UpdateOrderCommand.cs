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
    public class UpdateOrderCommand : IRequest<Unit>
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

        public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Unit>
        {
            IOrderRepository _orderRepository;
            private readonly IMapper _mapper;
            private readonly ILogger<UpdateOrderCommandHandler> _logger;

            public UpdateOrderCommandHandler(IOrderRepository repository, IMapper mapper, ILogger<UpdateOrderCommandHandler> logger)
            {
                _orderRepository = repository ?? throw new ArgumentNullException(nameof(repository));
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            }

            public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
            {
                var orderToUpdate = await _orderRepository.GetByIdAsync(request.Id);
                if (orderToUpdate == null)
                {
                    //throw new NotFoundException(nameof(Order), request.Id);
                }

                _mapper.Map(request, orderToUpdate, typeof(UpdateOrderCommand), typeof(Order));
                await _orderRepository.UpdateAsync(orderToUpdate);
                _logger.LogInformation($"Order { orderToUpdate.Id} is successfully updated.");
                return Unit.Value;
            }
        }
    }
}
