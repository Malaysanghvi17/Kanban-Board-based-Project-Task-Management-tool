using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Boards
{
    [Key]
    public int Bid { get; set; }

    [ForeignKey("Owner")]
    public int Uid { get; set; }  // Changed to OwnerId

    public string? Name { get; set; } // Made nullable

    public Users? Owner { get; set; } // Made nullable

    public List<Lane_Columns> Lanes { get; set; } = new();
}