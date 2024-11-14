	using Microsoft.EntityFrameworkCore;
    
    public class AppDbContext : DbContext
	{
	    public DbSet<Account> Accounts  { get; set; }
	
	    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	
	    {
	        optionsBuilder.UseSqlite(connectionString: "Data Source = Banco.sqlite");
	
	        optionsBuilder.LogTo(Console.WriteLine,LogLevel.Information);
	        
	        base.OnConfiguring(optionsBuilder);
	    }
	}
