using Microsoft.EntityFrameworkCore;
using MVCWithEntity.Models;

namespace MVCWithEntity.Reposatory;

public interface IDeptLearningRepo
{
    void Add(DeptLearning deptLearning);
    void Update(DeptLearning deptLearning);
    void Delete(DeptLearning deptLearning);
    DeptLearning? GetElement(int? courseId, int? deptId);
    List<DeptLearning> GetAll();
}

public class DeptLearningRepo : IDeptLearningRepo
{
    private readonly ItiDemoContext _dbContext;

    public DeptLearningRepo(ItiDemoContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Add(DeptLearning deptLearning)
    {
        _dbContext.DeptLearnings.Add(deptLearning);
        _dbContext.SaveChanges();
    }

    public void Update(DeptLearning deptLearning)
    {
        _dbContext.DeptLearnings.Update(deptLearning);
        _dbContext.SaveChanges();
    }

    public void Delete(DeptLearning deptLearning)
    {
        _dbContext.DeptLearnings.Remove(deptLearning);
        _dbContext.SaveChanges();
    }

    public DeptLearning? GetElement(int? courseId, int? deptId)
    {
        var deptLearning = _dbContext.DeptLearnings.FirstOrDefault(s => s.CourseId == courseId && s.DepartID == deptId);
        return deptLearning;
    }

    public List<DeptLearning> GetAll()
    {
        return _dbContext.DeptLearnings.Include(dl => dl.Course).Include(dl => dl.Department).ToList();
    }

}