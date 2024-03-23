using FluentValidation;

namespace MVCWithEntity.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public ICollection<UserRole> Roles { get; set; } = new HashSet<UserRole>();

}

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(u => u.Name)
            .NotEmpty().WithMessage("UserName is Required")
            .Length(5, 50).WithMessage("The Name Length is between 3, 50 Character long");

        RuleFor(u => u.Email)
            .NotEmpty().WithMessage("Email is Required")
            .EmailAddress().WithMessage("Enter Invalid Email");

        RuleFor(u => u.Password)
            .NotEmpty().WithMessage("UserName is Required")
            .Length(8, 20).WithMessage("Password lenght should More than 8 Charcter and less than 20");

    }
}