using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly JiraCloneContext _context;

    public UsersController(JiraCloneContext context)
    {
        _context = context;
    }

    // GET: api/users/{guid}
    [HttpGet("{guid}")]
    public IActionResult GetUser(int guid)
    {
        var user = _context.Users.Include(u => u.SharedBoards).FirstOrDefault(u => u.Uid == guid);
        if (user == null) return NotFound();
        return Ok(user);
    }

    // POST: api/users
    [HttpPost]
    public IActionResult CreateUser([FromBody] Users newUser)
    {
        _context.Users.Add(newUser);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetUser), new { guid = newUser.Uid }, newUser);
    }

    // PUT: api/users/{guid}
    [HttpPut("{guid}")]
    public IActionResult UpdateUser(int guid, [FromBody] Users updatedUser)
    {
        var user = _context.Users.FirstOrDefault(u => u.Uid == guid);
        if (user == null) return NotFound();

        user.Name = updatedUser.Name;
        user.Email = updatedUser.Email;
        user.CreatedAt = updatedUser.CreatedAt;

        _context.SaveChanges();
        return NoContent();
    }

    // DELETE: api/users/{guid}
    [HttpDelete("{guid}")]
    public IActionResult DeleteUser(int guid)
    {
        var user = _context.Users.FirstOrDefault(u => u.Uid == guid);
        if (user == null) return NotFound();

        _context.Users.Remove(user);
        _context.SaveChanges();
        return NoContent();
    }
}
