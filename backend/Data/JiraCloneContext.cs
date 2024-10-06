using Microsoft.EntityFrameworkCore;

public class JiraCloneContext : DbContext
{
    public JiraCloneContext(DbContextOptions<JiraCloneContext> options)
        : base(options)
    {
    }
    public DbSet<Users> Users { get; set; } 
    public DbSet<Boards> Boards { get; set; } 
    public DbSet<Lane_Columns> Lanes { get; set; } 
    public DbSet<Card_Issues> Cards { get; set; } 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Add any custom configurations here
        base.OnModelCreating(modelBuilder);
    }
}


// using Microsoft.EntityFrameworkCore;

// public class JiraCloneContext : DbContext
// {
//     public JiraCloneContext(DbContextOptions<JiraCloneContext> options) : base(options) { }

//     public DbSet<Users> Users { get; set; } 
//     public DbSet<Boards> Boards { get; set; } 
//     public DbSet<Lane_Columns> Lanes { get; set; } 
//     public DbSet<Card_Issues> Cards { get; set; } 
// }
