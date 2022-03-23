using AutoMapper;
using EventBus.Messages.Events;
using Ordering.Application.Features.Commands;

namespace Ordering.API.Mapper
{
    public class OrderingProfile : Profile
    {
        public OrderingProfile()
        {
            CreateMap<CheckOutOrderCommand, CartCheckoutEvent>().ReverseMap();
        }
    }
}
