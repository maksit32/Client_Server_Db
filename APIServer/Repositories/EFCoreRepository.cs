using APIServer.DbContexts;
using APIServer.Domain.Entities;
using APIServer.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APIServer.Repositories
{
	public class EFCoreRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
	{
		private readonly ServerDbContext _dbContext;

		private DbSet<TEntity> Entities => _dbContext.Set<TEntity>();

		public EFCoreRepository(ServerDbContext dbContext)
		{
			_dbContext = dbContext;
		}
	}
}
