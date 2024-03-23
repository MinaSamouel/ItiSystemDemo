using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCWithEntity.Models;
using MVCWithEntity.Reposatory;

namespace MVCWithEntity.Controllers;


public class CourseController : Controller
{
    private ICourseRepo _courseRepo ;
    private IValidator<Course> _validator;

    private CourseValidator validator = new CourseValidator(); 
    public CourseController(ICourseRepo courseRepo1, IValidator<Course> validator)
    {
        _courseRepo = courseRepo1;
        _validator = validator;
    }
    public IActionResult Index()
    {
        var courses = _courseRepo.GetAll();
        return View(courses);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Course course)
    {
        ValidationResult result = await _validator.ValidateAsync(course);

        if (!result.IsValid)
        {
            ModelState.AddModelError("", result.ToString());

            return View();
        }
        _courseRepo.Add(course);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Edit(int? id)
    {
        if (id == null) return BadRequest();
        var model = _courseRepo.GetElement(id);
        if (model == null) return NotFound();
        return View(model);
    }

    [HttpPost]
    public IActionResult Edit(Course course, int id)
    {
        var result = validator.Validate(course);
        if (!result.IsValid)
        {
            return View();
        }
        _courseRepo.Update(course);
        return RedirectToAction("Index");
    }

    public IActionResult Details(int? id)
    {
        if (id == null) return BadRequest();
        var model = _courseRepo.GetElement(id);
        if (model == null) return NotFound();
        return View(model);
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        var course = _courseRepo.GetElement(id);
        _courseRepo.Delete(course);
        return RedirectToAction("Index");
    }

    public IActionResult Details2(int? id)
    {
        if (id == null) return BadRequest();
        var model = _courseRepo.GetElement(id);
        if (model == null) return NotFound();
        return PartialView(model);
    }


}