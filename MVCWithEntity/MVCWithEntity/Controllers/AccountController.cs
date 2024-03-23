using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MVCWithEntity.Models;
using MVCWithEntity.Reposatory;

namespace MVCWithEntity.Controllers;

public class AccountController : Controller
{
    private readonly IUserRepo _userRepo;

    public AccountController(IUserRepo userRepo)
    {
        _userRepo = userRepo;
    }
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(User user)
    {
        var result = new UserValidator().Validate(user);
        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.ErrorMessage);
            }
            return View();
        }

        _userRepo.Add(user);
        return RedirectToAction("Index","Home");
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        var result = new LoginValidator().Validate(model);
        if (!result.IsValid)
        {
            ModelState.AddModelError("", "Invalid Email Or Password");
            return View(model);
        }
        var user = _userRepo.FindOne(model);

        if (user == null)
        {
            ModelState.AddModelError("", "Invalid Email Or Password");
            return View(model);
        }

        Claim c1 = new Claim(ClaimTypes.Name, user.Name);
        Claim c2 = new Claim(ClaimTypes.Email, user.Email);
        //List<Claim> c3 = new List<Claim>();
        
        //if (user.Roles.Count != 0)
        //{
        //    foreach (var role in user.Roles)
        //    {
        //        c3.Add(new Claim(ClaimTypes.Role, role.Role.Name));
        //    }
        //}
        
        Claim c3 = new Claim(ClaimTypes.Role, user.Roles.FirstOrDefault(u => u.UserId == user.Id).ToString());
        

        ClaimsIdentity identity = new ClaimsIdentity("Cookies");
        identity.AddClaim(c1);
        identity.AddClaim(c2);
        identity.AddClaim(c3);

        ClaimsPrincipal principal = new ClaimsPrincipal(identity);
        
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true
        };

        await HttpContext.SignInAsync(principal, authProperties);

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("Cookies");
        return RedirectToAction("Index", "Home");
    }
}