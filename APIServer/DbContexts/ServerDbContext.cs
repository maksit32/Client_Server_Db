using APIServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace APIServer.DbContexts
{
	public class ServerDbContext : DbContext
	{
		public DbSet<User> Users => Set<User>();
		public DbSet<Message> Messages => Set<Message>();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			//cascade deleting
			modelBuilder.Entity<Message>()
			.HasOne(e => e.User)
			.WithMany(u => u.Messages)
			.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<User>()
				.HasIndex(u => u.Login)
				.IsUnique();

			base.OnModelCreating(modelBuilder);
		}
		public ServerDbContext(DbContextOptions<ServerDbContext> options) : base(options)
		{
		}
	}
}
