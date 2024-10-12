using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Lane_Columns
{
    [Key]
    public int Lid { get; set; }

    public int Bid { get; set; }

    [ForeignKey("Bid")]
    public Boards? Board { get; set; } // Made nullable

    public string? Title { get; set; } // Made nullable
    public string? Label { get; set; } // Made nullable

    public List<Card_Issues> Cards { get; set; } = new();
}