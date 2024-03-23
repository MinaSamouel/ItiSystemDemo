using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCWithEntity.Models;
using MVCWithEntity.Reposatory;

namespace MVCWithEntity.Controllers;

public class StudentController : Controller
{
    //ItiDemoContext dbContext = new ItiDemoContext();
    private IDepartmentRepo _departmentRepo;
    private IStudentRepo _studentRepo ;
    private readonly IValidator<Student> _validator;

    StudentValidator validator = new StudentValidator();

    public StudentController(IDepartmentRepo departmentRepo, IStudentRepo studentRepo, IValidator<Student> validator)
    {
        _departmentRepo = departmentRepo;
        _studentRepo = studentRepo;
        _validator = validator;
    }

    public IActionResult Index()
    {
        var students = _studentRepo.GetAll();
        return View(students);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var departments = _departmentRepo.GetAll();
        ViewBag.deptlist = departments;
        return View();
    }

    //[HttpPost]
    //public async Task<IActionResult> Add(Student student, IFormFile studentImage)
    //{
    //    dbContext.Students.Add(student);
    //    dbContext.SaveChanges();

    //    var extName = studentImage.FileName.Split('.').Last();
    //    using (var fs = new FileStream($@"D:\MVCWithEntity\MVCWithEntity\wwwroot\Images\{student.Id}.{extName}", FileMode.Add))
    //    {
    //        await studentImage.CopyToAsync(fs);
    //    }
    //    return RedirectToAction("Index");
    //}

    [HttpPost]
    public IActionResult Create(Student student)
    {
        var result = _validator.Validate(student);
        if (!result.IsValid)
        {
            ModelState.AddModelError("",result.ToString());
            var departments = _departmentRepo.GetAll();
            ViewBag.deptlist = departments;
            return View();
        }
        _studentRepo.Add(student);

        return RedirectToAction("Index");
    }

    public IActionResult Details(int? id)
    {
        if (id == null) return BadRequest();
        var model = _studentRepo.GetElement(id);
        if (model == null) return NotFound();
        var departments = _departmentRepo.GetAll();
        ViewBag.deptlist = departments;
        return View(model);
    }

    [HttpGet]
    public IActionResult Edit(int? id)
    {
        if (id == null) return BadRequest();
        var model = _studentRepo.GetElement(id);
        if (model == null) return NotFound();
        var departments = _departmentRepo.GetAll();
        ViewBag.deptlist = departments;
        return View(model);
    }

    [HttpPost]
    public IActionResult Edit(Student student, int id)
    {
        //var oldStudents = dbContext.Students.ToList();
        
        //if (oldStudent == null || oldStudent == student) return RedirectToAction("Index");
        var result = validator.Validate(student);
        if (!result.IsValid)
        {
            ModelState.AddModelError("", result.ToString());
            var departments = _departmentRepo.GetAll();
            ViewBag.deptlist = departments;
            return View(student);
        }
        _studentRepo.Update(student);
        return RedirectToAction("Index");
    }

    public IActionResult Delete(int? id)
    {
        if (id == null) return BadRequest();
        var model = _studentRepo.GetElement(id);
        if (model == null) return NotFound();

        _studentRepo.Delete(model);

        return RedirectToAction("Index");
    }

    public IActionResult Details2(int? id)
    {
        if (id == null) return BadRequest();

        var model = _studentRepo.GetElement(id);

        if (model == null) return NotFound();
        return PartialView(model);
    }
}