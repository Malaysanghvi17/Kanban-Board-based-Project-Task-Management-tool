using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/lanes/{glid}/cards")]
public class CardIssuesController : ControllerBase
{
    private readonly JiraCloneContext _context;

    public CardIssuesController(JiraCloneContext context)
    {
        _context = context;
    }

    // GET: api/lanes/{glid}/cards
    [HttpGet]
    public IActionResult GetCards(int glid)
    {
        var cards = _context.Cards.Where(c => c.Lid == glid).ToList();
        return Ok(cards);
    }

    // POST: api/lanes/{glid}/cards
    [HttpPost]
    public IActionResult CreateCard(int glid, [FromBody] Card_Issues newCard)
    {
        newCard.Lid = glid;
        _context.Cards.Add(newCard);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetCards), new { glid }, newCard);
    }

    // PUT: api/lanes/{glid}/cards/{cid}
    [HttpPut("{cid}")]
    public IActionResult UpdateCard(int glid, int cid, [FromBody] Card_Issues updatedCard)
    {
        var card = _context.Cards.FirstOrDefault(c => c.Cid == cid);
        if (card == null) return NotFound();

        card.Title = updatedCard.Title;
        card.Label = updatedCard.Label;
        card.Description = updatedCard.Description;
        card.Comments = updatedCard.Comments;
        card.StartDate = updatedCard.StartDate;
        card.EndDate = updatedCard.EndDate;
        card.Lid = updatedCard.Lid;
        _context.SaveChanges();
        return NoContent();
    }

    // DELETE: api/lanes/{glid}/cards/{cid}
    [HttpDelete("{cid}")]
    public IActionResult DeleteCard(int glid, int cid)
    {
        var card = _context.Cards.FirstOrDefault(c => c.Lid == glid && c.Cid == cid);
        if (card == null) return NotFound();

        _context.Cards.Remove(card);
        _context.SaveChanges();
        return NoContent();
    }


    
}
