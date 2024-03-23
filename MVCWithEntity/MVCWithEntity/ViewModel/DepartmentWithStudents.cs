using MVCWithEntity.Models;

namespace MVCWithEntity.ViewModel
{
    public class StudentsWithCourse
    {
        public List<Student> Students { get; set; }

        public int CourseId { get; set; }
    }

    public class DepartmentWithStudents
    {
        public Department Department { get; set; }

        public List<Student> StudentsEnrolled { get; set; }

        public List<Student> StudentsNotEnrolled { get; set; }
    }
}
