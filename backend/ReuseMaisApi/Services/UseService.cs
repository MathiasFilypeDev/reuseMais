using Microsoft.EntityFrameworkCore;
using ReuseMaisApi.Models;

public class UserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public User? GetUserByUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            return null;

        return _context.Set<User>()
            .AsNoTracking()
            .FirstOrDefault(u => string.Equals((string?)u.Username, username, StringComparison.CurrentCultureIgnoreCase));
    }

    public User? GetUserById(int id)
    {
        return _context.Set<User>()
            .AsNoTracking()
            .FirstOrDefault(u => u.Id == id);
    }
}