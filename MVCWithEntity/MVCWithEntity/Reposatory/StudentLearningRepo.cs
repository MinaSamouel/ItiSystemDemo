using MVCWithEntity.Models;

namespace MVCWithEntity.Reposatory;

public interface IStudentLearningRepo
{
    void Add(StudentLearning studentLearning);
    void Update(StudentLearning studentLearning);
    void Delete(StudentLearning studentLearning);
    StudentLearning? GetElement(int? courseId, int? studentId);
    List<StudentLearning> GetAll();
}

public class StudentLearningRepo : IStudentLearningRepo
{
    private readonly ItiDemoContext _dbContext;

    public StudentLearningRepo(ItiDemoContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Add(StudentLearning studentLearning)
    {
        _dbContext.StudentLearnings.Add(studentLearning);
        _dbContext.SaveChanges();
    }

    public void Update(StudentLearning studentLearning)
    {
        _dbContext.StudentLearnings.Update(studentLearning);
        _dbContext.SaveChanges();
    }

    public void Delete(StudentLearning studentLearning)
    {
        _dbContext.StudentLearnings.Remove(studentLearning);
        _dbContext.SaveChanges();
    }

    public StudentLearning? GetElement(int? courseId, int? studentId)
    {
        var studentLearning = _dbContext.StudentLearnings.FirstOrDefault(s => s.CourseId == courseId && s.StudentID == studentId);
        return studentLearning;
    }

    public List<StudentLearning> GetAll()
    {
        return _dbContext.StudentLearnings.ToList();
    }

}