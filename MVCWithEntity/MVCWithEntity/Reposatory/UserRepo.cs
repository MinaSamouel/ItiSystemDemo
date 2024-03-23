using Microsoft.EntityFrameworkCore;
using MVCWithEntity.Models;

namespace MVCWithEntity.Reposatory;

public interface IUserRepo
{
    void Add(User user);
    User? FindOne(LoginViewModel user);

}

public class UserRepo : IUserRepo
{
    private readonly ItiDemoContext _context;

    public UserRepo(ItiDemoContext context)
    {
        _context = context;
    }

    public void Add(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public User? FindOne(LoginViewModel user)
    {
        return _context.Users.Include(u => u.Roles).FirstOrDefault(u => u.Email == user.Email && u.Password == user.Password);

    }
}