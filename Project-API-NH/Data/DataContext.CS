using System;


namespace Project_API_NH.Data
{
	public class DataContext: DbContext
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options)
		{

		}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
			optionsBuilder
				.UseSqlServer("Data Source=localhost,1433; Database=salesNH;User Id=sa;Password=Password.1;TrustServerCertificate=true");
        }

		public DbSet<User> Users => Set<User>();
	}
}

