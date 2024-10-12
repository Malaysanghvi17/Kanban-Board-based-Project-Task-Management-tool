using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/users/{guid}/boards")]
public class BoardsController : ControllerBase
{
    private readonly JiraCloneContext _context;

    public BoardsController(JiraCloneContext context)
    {
        _context = context;
    }

    // GET: api/users/{guid}/boards
    [HttpGet]
    public IActionResult GetBoards(int guid)
    {
        var boards = _context.Boards.Include(b => b.Lanes).Where(b => b.Uid == guid).ToList();
        return Ok(boards);
    }

    // POST: api/users/{guid}/boards
    [HttpPost]
    public IActionResult CreateBoard(int guid, [FromBody] Boards newBoard)
    {
        newBoard.Uid = guid;
        _context.Boards.Add(newBoard);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetBoards), new { guid }, newBoard);
    }

    // PUT: api/users/{guid}/boards/{bid}
    [HttpPut("{bid}")]
    public IActionResult UpdateBoard(int guid, int bid, [FromBody] Boards updatedBoard)
    {
        var board = _context.Boards.FirstOrDefault(b => b.Uid == guid && b.Bid == bid);
        if (board == null) return NotFound();

        board.Name = updatedBoard.Name;
        _context.SaveChanges();
        return NoContent();
    }

    // DELETE: api/users/{guid}/boards/{bid}
    [HttpDelete("{bid}")]
    public IActionResult DeleteBoard(int guid, int bid)
    {
        var board = _context.Boards.FirstOrDefault(b => b.Uid == guid && b.Bid == bid);
        if (board == null) return NotFound();

        _context.Boards.Remove(board);
        _context.SaveChanges();
        return NoContent();
    }
}
