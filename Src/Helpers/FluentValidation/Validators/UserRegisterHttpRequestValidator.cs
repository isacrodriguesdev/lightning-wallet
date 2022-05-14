using FluentValidation;
using Protocol;

namespace Validator
{                                              
  public class UserRegisterHttpRequestValidator : AbstractValidator<UserRegisterHttpRequest>
  {

    public UserRegisterHttpRequestValidator()
    {
      RuleFor(user => user.Name)
        .NotEmpty()
        .MaximumLength(21)
        .MinimumLength(3);

      RuleFor(user => user.Password)
      .NotEmpty()
      .Matches(@"\S\z")
      .MaximumLength(256)
      .MinimumLength(6);

      RuleFor(user => user.PhoneNumber)
      .Matches(@"^([0-9]{13,15})$");

      RuleFor(user => user.Email)
        .NotEmpty()
        .EmailAddress();

      RuleFor(user => user.Username)
        .Matches(@"\A\S{3,15}\z");
    }
  }
}