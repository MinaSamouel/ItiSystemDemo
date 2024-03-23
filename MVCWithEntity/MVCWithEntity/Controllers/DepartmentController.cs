using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCWithEntity.Models;
using MVCWithEntity.Reposatory;
using MVCWithEntity.ViewModel;

namespace MVCWithEntity.Controllers;

public class DepartmentController : Controller
{
    private IDepartmentRepo _departmentRepo;

    readonly IStudentRepo _studentRepo;
    //ItiDemoContext dbContext = new ItiDemoContext();
    private ICourseRepo _courseRepo;
    private IDeptLearningRepo _deptLearningRepo;
    private readonly IStudentLearningRepo _studentLearningRepo;
    private readonly IValidator<Department> _validator;

    DepartmentValidator departmentValidator = new DepartmentValidator();

    public DepartmentController(IDepartmentRepo departmentRepo, IStudentRepo studentRepo, ICourseRepo courseRepo, IDeptLearningRepo deptLearningRepo, IStudentLearningRepo studentLearningRepo, IValidator<Department> validator)
    {
        _departmentRepo = departmentRepo;
        _studentRepo = studentRepo;
        _courseRepo = courseRepo;
        _deptLearningRepo = deptLearningRepo;
        _studentLearningRepo = studentLearningRepo;
        _validator = validator;
    }

    public IActionResult Index()
    {
        var depts = _departmentRepo.GetAll();

        return View(depts);

    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();

    }

    [HttpPost]
    public IActionResult Create(Department department)
    {
        var result = _validator.Validate(department);
        if (!result.IsValid)
        {
            ModelState.AddModelError("", result.ToString());
            return View();
        }
        
        _departmentRepo.Add(department);

        return RedirectToAction("Index");
    }

    public IActionResult Details(int? id)
    {
        if (id == null) return BadRequest();
        var model = _departmentRepo.GetElement(id);
        if (model == null) return NotFound();
        return View("UpdateDetail", model);
    }

    [HttpGet]
    public IActionResult Update(int? id)
    {
        if(id == null) return BadRequest();
        var model = _departmentRepo.GetElement(id);
        if (model == null) return NotFound();
        return View(model);
    }

    [HttpPost]
    public IActionResult Update(Department department, int id)
    {
        var oldDepartment = _departmentRepo.GetElement(id);

        var result = departmentValidator.Validate(department);
        if (!result.IsValid)
        {
            ModelState.AddModelError("", result.ToString());
            return View();
        }
        oldDepartment.Name = department.Name; 
        _departmentRepo.Update(oldDepartment);
        return RedirectToAction("Index");

    }

    public IActionResult Delete(int? id)
    {
        if (id == null) return BadRequest();
        var model = _departmentRepo.GetElement(id);
        if (model == null) return NotFound();
        _departmentRepo.Delete(model);
        return RedirectToAction("Index");
    }

    public IActionResult Details2(int? id)
    {
        if (id == null) return BadRequest();
        var model = _departmentRepo.GetElement(id);
        if (model == null) return NotFound();
        return PartialView(model);
    }

    public IActionResult ManageStudent(int? id)
    {
        if (id == null) return BadRequest();
        var model = _departmentRepo.GetElement(id);
        if (model == null) return NotFound();

        var deptWithStudents = new DepartmentWithStudents
        {
            Department = model,
            StudentsEnrolled = _studentRepo.GetAll().Where(s => s.DepartmentId == id).ToList(),
            StudentsNotEnrolled = _studentRepo.GetAll().Where(s => s.Department == null).ToList()
        };

        return View(deptWithStudents);
    }

    public IActionResult RemoveFromDepartment(int? studentId, int departmentId)
    {
        if (studentId == null) return BadRequest();

        var student = _studentRepo.GetElement(studentId);
        if (student == null) return NotFound();
        student.DepartmentId = null;
        _studentRepo.Update(student);
        return RedirectToAction("ManageStudent", new { id = departmentId });
    }

    public IActionResult AddToDepartment(int? studentId, int departmentId)
    {
        if (studentId == null) return BadRequest();

        var student = _studentRepo.GetElement(studentId);
        if (student == null) return NotFound();
        var department = _departmentRepo.GetElement(departmentId);
        student.DepartmentId = departmentId;
        _studentRepo.Update(student);

        return RedirectToAction("ManageStudent", new { id = departmentId });
    }

    public IActionResult ManageCourses(int? id)
    {
        if (id == null) return BadRequest();
        var model = _departmentRepo.GetElement(id);
        if (model == null) return NotFound();

        var deptWithCourses = new DepartmentWithCourses { };

        deptWithCourses.Department = model;

        deptWithCourses.CoursesInsideDepartment =
            _deptLearningRepo.GetAll().Where(s => s.DepartID == id).Select(s => s.Course).ToList();

        var checkCourses = _deptLearningRepo.GetAll().Where(s => s.DepartID == id).Select(s => s.Course).ToList();

        //deptWithCourses.CoursesNotInsideDepartment = checkCourses.Count == 0 ? dbContext.Courses.ToList() : checkCourses;
        deptWithCourses.CoursesNotInsideDepartment = _courseRepo.GetAll();

        var wholeCourse = _courseRepo.GetAll();

        foreach (var checkCourse in checkCourses)
        {
            var course = deptWithCourses.CoursesNotInsideDepartment.FirstOrDefault(c => c.Id == checkCourse.Id);

            if (deptWithCourses.CoursesNotInsideDepartment.Contains(course))
            {
                deptWithCourses.CoursesNotInsideDepartment.Remove(course);
            }
        }

        return View(deptWithCourses);
    }

    public IActionResult RemoveCourseFromDepartment(int? courseId, int departmentId)
    {
        if (courseId == null) return BadRequest();
        var course = _deptLearningRepo.GetElement(courseId, departmentId);
        if (course == null) return NotFound();

        var deptLearn = _deptLearningRepo.GetElement(courseId, departmentId);

        _deptLearningRepo.Delete(course);
        return RedirectToAction("ManageCourses", new { id = departmentId });
    }

    public IActionResult AddCourseToDepartment(int? courseId, int departmentId)
    {
        if (courseId == null) return BadRequest();
        var course = _courseRepo.GetElement(courseId);
        if (course == null) return NotFound();

        var deptLearn = new DeptLearning { CourseId = courseId.Value, DepartID = departmentId };
        _deptLearningRepo.Add(deptLearn);

        return RedirectToAction("ManageCourses", new { id = departmentId });
    }

    [HttpGet]
    public IActionResult ManageStudentsGrade(int departmentId, int courseId)
    {
        var department = _departmentRepo.GetElement(departmentId);

        var course = _courseRepo.GetElement(courseId);

        ViewBag.course = course!;
        var grades = new List<int?>();
        foreach (var student in department!.Students)
        {
            var studentCourse = new StudentLearning() { CourseId = courseId, StudentID = student.Id };

            if (_studentLearningRepo.GetAll().Contains(studentCourse))
            {
                grades.Add(studentCourse.Grade);
            }
            else
                grades.Add(null);
        }
        ViewBag.grades = grades;

        return View(department);

    }

    [HttpPost]
    public IActionResult ManageStudentsGrade(int departmentId, int courseId, Dictionary<int, int?> degree)
    {

        foreach (var item in degree)
        {
            //var studentCourse = new StudentLearning() { CourseId = courseId, StudentID = item.Key, Grade = item.Value };
            var studentCourse = _studentLearningRepo.GetElement(courseId, item.Key);
            if (_studentLearningRepo.GetAll().Contains(studentCourse))
            {
                _studentLearningRepo.Update(studentCourse);
            }
            else
                _studentLearningRepo.Add(studentCourse);
        }

        return RedirectToAction("ManageCourses", "Department", new { id = departmentId });
    }


}


