namespace MVCWithEntity.Models;

public class StudentLearning
{
    public int CourseId { get; set; }
    public int StudentID { get; set; }
    public int? Grade { get; set; }

    public Student Student { get; set; }
    public Course Course { get; set; }
}