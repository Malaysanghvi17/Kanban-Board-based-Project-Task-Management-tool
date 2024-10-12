using System.ComponentModel.DataAnnotations;

public class Card_Issues
{
    [Key]
    public int Cid { get; set; }
    public int Pid { get; set; }
    public string? Title { get; set; } // Made nullable
    public string? Label { get; set; } // Made nullable
    public string? Description { get; set; } // Made nullable
    public string? Comments { get; set; } // Made nullable
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Lid { get; set; }
    public Lane_Columns? Lane { get; set; } // Made nullable
}