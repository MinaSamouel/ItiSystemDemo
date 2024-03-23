using MVCWithEntity.Models;

namespace MVCWithEntity.ViewModel;


public class DepartmentWithCourses
{
    public Department Department { get; set; }

    public List<Course> CoursesInsideDepartment { get; set; }

    public List<Course> CoursesNotInsideDepartment { get; set; }
}