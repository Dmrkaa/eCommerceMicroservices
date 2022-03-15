using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Commands
{
    public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
        {
            RuleFor(p => p.UserName).NotEmpty().WithMessage("{UserName} is required.")
              .NotNull()
              .MaximumLength(30).WithMessage("UserName not over 30 characters.");

            RuleFor(p => p.EmailAddress).NotEmpty().WithMessage("{EmailAddress} is required.").EmailAddress();

            RuleFor(p => p.TotalPrice).NotEmpty().WithMessage("{ TotalPrice} is required.")
                                      .GreaterThan(0).WithMessage("{ TotalPrice}should be greater than zero.");
        }
    }
}
