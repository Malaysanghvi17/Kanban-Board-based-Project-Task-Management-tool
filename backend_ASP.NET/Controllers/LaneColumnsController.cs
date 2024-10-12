using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/boards/{gbid}/lanes")]
public class LaneColumnsController : ControllerBase
{
    private readonly JiraCloneContext _context;

    public LaneColumnsController(JiraCloneContext context)
    {
        _context = context;
    }

    // GET: api/boards/{gbid}/lanes
    [HttpGet]
    public IActionResult GetLanes(int gbid)
    {
        var lanes = _context.Lanes.Include(l => l.Cards).Where(l => l.Bid == gbid).ToList();
        return Ok(lanes);
    }

    // POST: api/boards/{gbid}/lanes
    [HttpPost]
    public IActionResult CreateLane(int gbid, [FromBody] Lane_Columns newLane)
    {
        newLane.Bid = gbid;
        _context.Lanes.Add(newLane);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetLanes), new { gbid }, newLane);
    }

    // PUT: api/boards/{gbid}/lanes/{lid}
    [HttpPut("{lid}")]
    public IActionResult UpdateLane(int gbid, int lid, [FromBody] Lane_Columns updatedLane)
    {
        var lane = _context.Lanes.FirstOrDefault(l => l.Bid == gbid && l.Lid == lid);
        if (lane == null) return NotFound();

        lane.Title = updatedLane.Title;
        lane.Label = updatedLane.Label;
        _context.SaveChanges();
        return NoContent();
    }

    // DELETE: api/boards/{gbid}/lanes/{lid}
    [HttpDelete("{lid}")]
    public IActionResult DeleteLane(int gbid, int lid)
    {
        var lane = _context.Lanes.FirstOrDefault(l => l.Bid == gbid && l.Lid == lid);
        if (lane == null) return NotFound();

        _context.Lanes.Remove(lane);
        _context.SaveChanges();
        return NoContent();
    }
}
