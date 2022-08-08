using FluentValidation;
using FluentValidation.Results;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_Task_3.DTOs.Accounts
{
    public class RegisterDto
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

    }

    public class RegisterDtoValidation : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidation()
        {
            RuleFor(x => x.Firstname).MaximumLength(30).WithMessage("max length 30").NotEmpty();
            RuleFor(x => x.Lastname).MaximumLength(30).WithMessage("max length 30").NotEmpty();
            RuleFor(x => x.Username).MaximumLength(20).WithMessage("max length 20").NotEmpty();
            RuleFor(x => x.Email).MaximumLength(30).WithMessage("max length 30").NotEmpty().EmailAddress(mode:EmailValidationMode.AspNetCoreCompatible);
            RuleFor(x => x).Custom((x, context) =>
            {
                if(x.Password != x.ConfirmPassword)
                {
                    context.AddFailure(new ValidationFailure("Password", "Password and confirm password must match"));
                }

            });
        }
    }
}
