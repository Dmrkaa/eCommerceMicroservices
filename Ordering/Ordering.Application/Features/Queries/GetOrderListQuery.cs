using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Features.Dtos;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Queries
{
    public class GetOrderListQuery : IRequest<List<OrderDto>>
    {
        public string UserName { get; set; }

        public GetOrderListQuery(string userName)
        {
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
        }

        public class GetOrderListQueryHandler : IRequestHandler<GetOrderListQuery, List<OrderDto>>
        {
            private readonly IOrderRepository _orderRepository;
            private readonly IMapper _mapper;
            public GetOrderListQueryHandler(IOrderRepository repository, IMapper mapper)
            {
                _orderRepository = repository ?? throw new ArgumentNullException(nameof(_orderRepository));
                _mapper = mapper ?? throw new ArgumentNullException(nameof(_mapper));
            }

            public async Task<List<OrderDto>> Handle(GetOrderListQuery request, CancellationToken cancellationToken)
            {
                var orders = await _orderRepository.GetOrdersByUserName(request.UserName);
                return _mapper.Map<List<OrderDto>>(orders);
            }
        }

    }
}
