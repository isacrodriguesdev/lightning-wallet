using FluentValidation;
using Protocol;

namespace Validator
{
  public class CreateInvoiceHttpRequestValidator : AbstractValidator<CreateInvoiceHttpRequest>
  {
    public CreateInvoiceHttpRequestValidator()
    {
      RuleFor(invoice => invoice.Amount)
        .GreaterThanOrEqualTo(0);
    }
  }
}