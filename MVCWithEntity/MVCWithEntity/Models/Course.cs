using FluentValidation;

namespace MVCWithEntity.Models;

public class Course
{
    public int Id { get; set; }
    public string Name { get; set; }

    public int Hours { get; set; }

    public ICollection<DeptLearning> DeptLearning { get; set; } = new HashSet<DeptLearning>();

    public ICollection<StudentLearning> StudentLearnings { get; set; } = new HashSet<StudentLearning>();
}

public class CourseValidator : AbstractValidator<Course>
{
    public CourseValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Course name is required")
            .Length(3, 20).WithMessage("Course name must be between 3 and 20 characters long");

        RuleFor(c => c.Hours)
            .NotEmpty().WithMessage("Course hours is required")
            .InclusiveBetween(10, 100).WithMessage("Course hours must be between 10 and 100");
    }
}