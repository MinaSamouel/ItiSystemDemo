using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace MVCWithEntity.Models;

public class LoginViewModel
{
    [Key]
    [EmailAddress]
    public string Email { get; set; }
    public string Password { get; set; }
}

public class LoginValidator : AbstractValidator<LoginViewModel>
{
    public LoginValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty().WithMessage("Email is Required")
            .EmailAddress().WithMessage("Enter Invalid Email");

        RuleFor(u => u.Password)
            .NotEmpty().WithMessage("Password is Required");
    }
}