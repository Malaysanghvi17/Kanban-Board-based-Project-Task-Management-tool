using System.ComponentModel.DataAnnotations;

public class Users
{
    [Key]
    public int Uid { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public List<Boards> SharedBoards { get; set; } = new();

    public DateTimeOffset CreatedAt { get; set; }
}
