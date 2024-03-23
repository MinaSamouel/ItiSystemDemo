using FluentValidation;

namespace MVCWithEntity.Models;

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public int? Age { get; set; }
    public int? DepartmentId { get; set; }
    public Department? Department { get; set; }
    public ICollection<StudentLearning> StudentLearnings { get; set; } = new HashSet<StudentLearning>();
}

public class StudentValidator : AbstractValidator<Student>
{
    public StudentValidator()
    {
        RuleFor(s => s.Name)
            .NotEmpty().WithMessage("Student name is required")
            .Length(3, 20).WithMessage("Student name must be between 3 and 20 characters long");

        RuleFor(s => s.Email)
            .NotEmpty().WithMessage("Student email is required")
            .EmailAddress().WithMessage("Student email is not valid");

        RuleFor(s => s.Age)
            .NotEmpty().WithMessage("Student age is required")
            .InclusiveBetween(18, 60).WithMessage("Student age must be between 18 and 60");
    }
}
