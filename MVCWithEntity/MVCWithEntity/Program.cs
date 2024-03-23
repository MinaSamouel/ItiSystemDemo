using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using MVCWithEntity.Models;
using MVCWithEntity.Reposatory;

namespace MVCWithEntity
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<ICourseRepo, CourseRepo>();
            builder.Services.AddScoped<IStudentRepo, StudentRepo>();
            builder.Services.AddScoped<IDepartmentRepo, DepartmentRepo>();
            builder.Services.AddScoped<IStudentLearningRepo, StudentLearningRepo>();
            builder.Services.AddScoped<IDeptLearningRepo, DeptLearningRepo>();
            builder.Services.AddScoped<IUserRepo, UserRepo>();

            builder.Services.AddDbContext<ItiDemoContext>(o =>
            {
                o.UseSqlServer(builder.Configuration.GetConnectionString("Database1"));
            });

            builder.Services.AddMvc();
            builder.Services.AddFluentValidationClientsideAdapters();
            builder.Services.AddScoped<IValidator<Course>, CourseValidator>();
            builder.Services.AddScoped<IValidator<Department>, DepartmentValidator>();
            builder.Services.AddScoped<IValidator<Student>, StudentValidator>();

            builder.Services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();
            
            builder.Services.AddSession();

            var app = builder.Build();
            
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
