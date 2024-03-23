using Microsoft.EntityFrameworkCore;
using MVCWithEntity.Models;

namespace MVCWithEntity.Models;

public class ItiDemoContext : DbContext
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<StudentLearning> StudentLearnings { get; set; }
    public DbSet<DeptLearning> DeptLearnings { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }

    public ItiDemoContext()
    {

    }

    public ItiDemoContext(DbContextOptions options): base(options)
    {
        
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=ItiDemo;Trusted_Connection=True;TrustServerCertificate=True");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region CreateDepartmentTable

        modelBuilder.Entity<Department>()
            .Property(d => d.Id)
            .ValueGeneratedNever()
            .HasColumnName("Department_Id");

        modelBuilder.Entity<Department>()
            .Property(d => d.Name)
            .HasColumnName("Department_Name")
            .HasColumnType("nvarchar(50)");

        modelBuilder.Entity<Department>()
            .HasMany(d => d.Students)
            .WithOne(s => s.Department)
            .HasForeignKey(s => s.Id);

        #endregion

        #region CreateStudentTable

        modelBuilder.Entity<Student>()
            .Property(s => s.Id)
            .HasColumnName("Student_Id");

        modelBuilder.Entity<Student>()
            .Property(s => s.Name)
            .HasColumnName("Student_Name")
            .HasColumnType("nvarchar(150)");

        modelBuilder.Entity<Student>()
            .Property(s => s.Age)
            .HasColumnName("Student_Age");

        modelBuilder.Entity<Student>()
            .Property(s => s.Email)
            .HasColumnType("nvarchar(200)")
            .HasConversion(
                s => s.ToLower(),
                s => s);

        modelBuilder.Entity<Student>()
            .HasOne(s => s.Department)
            .WithMany(d => d.Students)
            .HasForeignKey(s => s.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade);

        #endregion

        #region CreateCourseTable

        modelBuilder.Entity<Course>()
            .Property(c => c.Id)
            .HasColumnName("Course_Id");

        modelBuilder.Entity<Course>()
            .Property(c => c.Name)
            .HasColumnName("Course_Name")
            .HasColumnType("nvarchar(150)");

        modelBuilder.Entity<Course>()
            .Property(c => c.Hours)
            .HasColumnName("Total_Hours");

        #endregion

        #region CreateDeptLearningTable

        modelBuilder.Entity<DeptLearning>()
            .HasKey(dt => new { dt.DepartID, dt.CourseId });

        modelBuilder.Entity<DeptLearning>()
            .HasOne(dt => dt.Course)
            .WithMany(c => c.DeptLearning)
            .HasForeignKey(dt => dt.CourseId);

        modelBuilder.Entity<DeptLearning>()
            .HasOne(dt => dt.Department)
            .WithMany(d => d.DeptLearning)
            .HasForeignKey(dt => dt.DepartID);

        #endregion

        #region CreateStudentLearningTable

        modelBuilder.Entity<StudentLearning>()
            .HasKey(sl => new { sl.StudentID, sl.CourseId });

        modelBuilder.Entity<StudentLearning>()
            .HasOne(sl => sl.Student)
            .WithMany(s => s.StudentLearnings)
            .HasForeignKey(sl => sl.StudentID);

        modelBuilder.Entity<StudentLearning>()
            .HasOne(sl => sl.Course)
            .WithMany(c => c.StudentLearnings)
            .HasForeignKey(sl => sl.CourseId);

        #endregion

        #region CreatingUserTable

        modelBuilder.Entity<User>()
            .Property(u => u.Id)
            .HasColumnName("User_Id")
            .HasColumnOrder(1);

        modelBuilder.Entity<User>()
            .Property(u => u.Name)
            .HasColumnName("User_Name")
            .HasColumnType("nvarchar(50)");

        modelBuilder.Entity<User>()
            .Property(u => u.Email)
            .HasColumnType("nvarchar(50)");

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .Property(u => u.Password)
            .HasColumnType("nvarchar(150)");

        #endregion

        #region CreateRoleTable

        modelBuilder.Entity<Role>()
            .Property(r => r.Id)
            .HasColumnName("Role_Id");

        modelBuilder.Entity<Role>()
            .Property(r => r.Name)
            .HasColumnType("nvarchar(20)");

        #endregion

        #region CreateUserRoleTable

        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(ur => ur.RoleId);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.Roles)
            .HasForeignKey(u => u.UserId);

        modelBuilder.Entity<UserRole>()
            .Property(u => u.UserId)
            .HasColumnOrder(1);

        #endregion

    }

public DbSet<MVCWithEntity.Models.LoginViewModel> LoginViewModel { get; set; } = default!;

}