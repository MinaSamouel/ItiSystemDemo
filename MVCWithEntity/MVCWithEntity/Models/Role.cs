namespace MVCWithEntity.Models;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<UserRole> Users { get; set; } = new HashSet<UserRole>();
}

public class UserRole
{
    public int UserId { get; set; }
    public int RoleId { get; set; }

    public Role Role { get; set; }
    public User User { get; set; }
}