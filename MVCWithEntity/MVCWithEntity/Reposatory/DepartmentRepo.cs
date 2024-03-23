using Microsoft.EntityFrameworkCore;
using MVCWithEntity.Models;

namespace MVCWithEntity.Reposatory;

public interface IDepartmentRepo
{
    void Add(Department department);
    void Update(Department department);
    void Delete(Department department);
    Department? GetElement(int? id);
    List<Department> GetAll();
}

public class DepartmentRepo : IDepartmentRepo
{
    private readonly ItiDemoContext _dbContext;

    public DepartmentRepo(ItiDemoContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Add(Department department)
    {
        _dbContext.Departments.Add(department);
        _dbContext.SaveChanges();
    }

    public void Update(Department department)
    {
        _dbContext.Departments.Update(department);
        _dbContext.SaveChanges();
    }

    public void Delete(Department department)
    {
        _dbContext.Departments.Remove(department);
        _dbContext.SaveChanges();
    }

    public Department? GetElement(int? id)
    {
        var department = _dbContext.Departments.Include(d => d.Students).FirstOrDefault(s => s.Id == id);
        return department;
    }

    public List<Department> GetAll()
    {
        return _dbContext.Departments.ToList();
    }

}