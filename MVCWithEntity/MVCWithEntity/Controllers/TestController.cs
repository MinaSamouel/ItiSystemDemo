using Microsoft.AspNetCore.Mvc;

namespace MVCWithEntity.Controllers;

public class TestController : Controller
{
    string FirstName = "Ahmed";
    string LastName = "Ali";

    public IActionResult Index()
    {
        Response.Cookies.Append("FirstName", FirstName, new CookieOptions()
        {
            Secure = true
        });
        Response.Cookies.Append("LastName", LastName, new CookieOptions());
        return View();
    }

    public IActionResult Read()
    {
        var FirstName = Request.Cookies["FirstName"];
        var LastName = Request.Cookies["LastName"];

        return Content($@"After reading the cookies the FirstName: {FirstName} and the LastName: {LastName}");
    }

}