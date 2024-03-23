using Microsoft.EntityFrameworkCore;
using MVCWithEntity.Models;

namespace MVCWithEntity.Reposatory;

public interface ICourseRepo
{
    void Add(Course course);
    void Update(Course course);

    void Delete(Course course);

    Course? GetElement(int? id);

    List<Course> GetAll();
}

public class CourseRepo : ICourseRepo
{
    //ItiDemoContext dbContext = new ItiDemoContext();
    private ItiDemoContext _dbContext;

    public CourseRepo(ItiDemoContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Add(Course course)
    {
        _dbContext.Courses.Add(course);
        _dbContext.SaveChanges();
    }

    public void Update(Course course)
    {
        _dbContext.Courses.Update(course);
        _dbContext.SaveChanges();
    }

    public void Delete(Course course)
    {
        _dbContext.Courses.Remove(course);
        _dbContext.SaveChanges();
    }

    public Course? GetElement(int? id)
    {
        var course = _dbContext.Courses.FirstOrDefault(s => s.Id == id);
        return course;
    }

    public List<Course> GetAll()
    {
        return _dbContext.Courses.Include(c => c.DeptLearning).ToList();
    }

}