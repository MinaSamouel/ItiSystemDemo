using Microsoft.EntityFrameworkCore;
using MVCWithEntity.Models;

namespace MVCWithEntity.Reposatory;

public interface IStudentRepo
{
    void Add(Student student);
    void Update(Student student);
    void Delete(Student student);
    Student? GetElement(int? id);
    List<Student> GetAll();
}

public class StudentRepo : IStudentRepo
{
    private readonly ItiDemoContext _dbContext;

    public StudentRepo(ItiDemoContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Add(Student student)
    {
        _dbContext.Students.Add(student);
        _dbContext.SaveChanges();
    }

    public void Update(Student student)
    {
        _dbContext.Students.Update(student);
        _dbContext.SaveChanges();
    }

    public void Delete(Student student)
    {
        _dbContext.Students.Remove(student);
        _dbContext.SaveChanges();
    }

    public Student? GetElement(int? id)
    {
        var student = _dbContext.Students.Include(s=>s.Department).FirstOrDefault(s => s.Id ==id);
        //var student = students.FirstOrDefault(s => s.Id == id);
        return student;
    }

    public List<Student> GetAll()
    {
        return _dbContext.Students.Include(s =>s.Department).ToList();
    }
}

