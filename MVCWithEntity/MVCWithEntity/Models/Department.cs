using FluentValidation;

namespace MVCWithEntity.Models;

public class Department
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<Student> Students { get; set; } = new HashSet<Student>();

    public ICollection<DeptLearning> DeptLearning { get; set; } = new HashSet<DeptLearning>();
}

public class DepartmentValidator : AbstractValidator<Department>
{
    public DepartmentValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("Course hours is required\n")
            .Must(BeMultipleOf100).WithMessage("Department Number  must be a multiple of 100 like 100,200,300,....\n");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("\nDepartment name is required\n")
            .Length(8,50).WithMessage("\nDepartment name Should be between 8 and 50 letters\n");
    }
    private bool BeMultipleOf100(int hours)
    {
        // Your custom validation logic here
        return hours % 100 == 0;
    }
}

