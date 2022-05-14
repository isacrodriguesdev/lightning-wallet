using FluentValidation;
using Protocol;

namespace Validator
{
  public class SendPaymentHttpRequestValidator : AbstractValidator<SendPaymentHttpRequest>
  {
    public SendPaymentHttpRequestValidator()
    {
      RuleFor(payment => payment.PaymentRequest)
        .NotEmpty()
        .MinimumLength(24);

      RuleFor(payment => payment.Amount)
        .GreaterThanOrEqualTo(0);
    }
  }
}