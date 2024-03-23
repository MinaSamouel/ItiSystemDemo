namespace MVCWithEntity.Models;

public class DeptLearning
{
    public int CourseId { get; set; }
    public int DepartID { get; set; }

    public Course Course { get; set;}
    public Department Department { get; set;}
}