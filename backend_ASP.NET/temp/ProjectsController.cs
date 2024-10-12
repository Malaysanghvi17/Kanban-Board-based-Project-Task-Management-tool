using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// [EnableCors("MyAllowSpecificOrigins")]
[Route("api/[controller]")]
[ApiController]
public class ProjectsController : ControllerBase
{
    private readonly JiraCloneContext _context;

    public ProjectsController(JiraCloneContext context)
    {
        _context = context;
    }

    // ----------- Boards (Projects) Operations -----------

    [HttpGet("/")]
    public IActionResult HealthCheck()
    {
        return Ok("Hello World!\n");
    }
    // GET: api/projects?uid={ownerId}
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Boards>>> GetBoardsByUser(int ownerId)
    {
        return await _context.Boards
            .Where(b => b.Uid == ownerId)
            .Include(b => b.Lanes).ThenInclude(l => l.Cards)
            .ToListAsync();
    }

    // GET: api/projects/{id}?uid={ownerId}
    [HttpGet("{id}")]
    public async Task<ActionResult<Boards>> GetBoard(int id, int ownerId)
    {
        var board = await _context.Boards
            .Include(b => b.Lanes).ThenInclude(l => l.Cards)
            .FirstOrDefaultAsync(b => b.Bid == id && b.Uid == ownerId);

        if (board == null)
        {
            return NotFound();
        }

        return board;
    }

    // POST: api/projects
    [HttpPost]
    public async Task<IActionResult> CreateBoard([FromBody] Boards board)
    {
        if (board.Uid == 0)
        {
            return BadRequest("OwnerId (Uid) is required");
        }

        var owner = await _context.Users.FindAsync(board.Uid);
        if (owner == null)
        {
            return NotFound("Owner not found.");
        }

        board.Owner = owner; // Set the Owner object
        _context.Boards.Add(board);
        await _context.SaveChangesAsync();

        return Ok(board);
    }


    // PUT: api/projects/{id}?uid={ownerId}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBoard(int id, Boards board, int ownerId)
    {
        if (id != board.Bid || ownerId != board.Uid)
        {
            return BadRequest();
        }

        _context.Entry(board).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BoardExists(id, ownerId))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    // DELETE: api/projects/{id}?uid={ownerId}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBoard(int id, int ownerId)
    {
        var board = await _context.Boards.FirstOrDefaultAsync(b => b.Bid == id && b.Uid == ownerId);
        if (board == null)
        {
            return NotFound();
        }

        _context.Boards.Remove(board);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // ----------- Lane Operations -----------

    // GET: api/projects/lane?gbid={bid}
    [HttpGet("lane")]
    public async Task<ActionResult<IEnumerable<Lane_Columns>>> GetLanesByBoard(int gbid)
    {
        return await _context.Lanes
            .Where(l => l.Bid == gbid)
            .Include(l => l.Cards)
            .ToListAsync();
    }

    // GET: api/projects/lane/{id}?gbid={bid}
    [HttpGet("lane/{id}")]
    public async Task<ActionResult<Lane_Columns>> GetLane(int id, int gbid)
    {
        var lane = await _context.Lanes
            .FirstOrDefaultAsync(l => l.Lid == id && l.Bid == gbid);

        if (lane == null)
        {
            return NotFound();
        }

        return lane;
    }

    // POST: api/projects/lane
    [HttpPost("lane")]
    public async Task<ActionResult<Lane_Columns>> CreateLane(Lane_Columns lane)
    {
        _context.Lanes.Add(lane);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetLane), new { id = lane.Lid, gbid = lane.Bid }, lane);
    }

    // PUT: api/projects/lane/{id}?gbid={bid}
    [HttpPut("lane/{id}")]
    public async Task<IActionResult> UpdateLane(int id, Lane_Columns lane, int gbid)
    {
        if (id != lane.Lid || gbid != lane.Bid)
        {
            return BadRequest();
        }

        _context.Entry(lane).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!LaneExists(id, gbid))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    // DELETE: api/projects/lane/{id}?gbid={bid}
    [HttpDelete("lane/{id}")]
    public async Task<IActionResult> DeleteLane(int id, int gbid)
    {
        var lane = await _context.Lanes.FirstOrDefaultAsync(l => l.Lid == id && l.Bid == gbid);
        if (lane == null)
        {
            return NotFound();
        }

        _context.Lanes.Remove(lane);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // ----------- Card Operations -----------

    // GET: api/projects/cards?glid={lid}
    [HttpGet("cards")]
    public async Task<ActionResult<IEnumerable<Card_Issues>>> GetCardsByLane(int glid)
    {
        return await _context.Cards
            .Where(c => c.Lid == glid)
            .ToListAsync();
    }

    // GET: api/projects/cards/{id}?glid={lid}
    [HttpGet("cards/{id}")]
    public async Task<ActionResult<Card_Issues>> GetCard(int id, int glid)
    {
        var card = await _context.Cards
            .FirstOrDefaultAsync(c => c.Cid == id && c.Lid == glid);

        if (card == null)
        {
            return NotFound();
        }

        return card;
    }

    // POST: api/projects/cards
    [HttpPost("cards")]
    public async Task<ActionResult<Card_Issues>> CreateCard(Card_Issues card)
    {
        _context.Cards.Add(card);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCard), new { id = card.Cid, glid = card.Lid }, card);
    }

    // PUT: api/projects/cards/{id}?glid={lid}
    [HttpPut("cards/{id}")]
    public async Task<IActionResult> UpdateCard(int id, Card_Issues card, int glid)
    {
        if (id != card.Cid || glid != card.Lid)
        {
            return BadRequest();
        }

        _context.Entry(card).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CardExists(id, glid))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    // DELETE: api/projects/cards/{id}?glid={lid}
    [HttpDelete("cards/{id}")]
    public async Task<IActionResult> DeleteCard(int id, int glid)
    {
        var card = await _context.Cards.FirstOrDefaultAsync(c => c.Cid == id && c.Lid == glid);
        if (card == null)
        {
            return NotFound();
        }

        _context.Cards.Remove(card);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // Utility methods
    private bool BoardExists(int id, int ownerId)
    {
        return _context.Boards.Any(e => e.Bid == id && e.Uid == ownerId);
    }

    private bool LaneExists(int id, int gbid)
    {
        return _context.Lanes.Any(e => e.Lid == id && e.Bid == gbid);
    }

    private bool CardExists(int id, int glid)
    {
        return _context.Cards.Any(e => e.Cid == id && e.Lid == glid);
    }
}
