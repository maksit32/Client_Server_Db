using APIServer.DbContexts;
using APIServer.Domain.Entities;
using APIServer.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace APIServer.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly ServerDbContext _dbContext;
		private DbSet<User> Users => _dbContext.Set<User>();

		public UserRepository(ServerDbContext dbContext)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
		}

		public async Task<bool> AddUserAsync(User user, CancellationToken ct)
		{
			if (user is null)
				throw new ArgumentNullException(nameof(user));

			//уже зарегистрирован
			if (Users.ToList().Exists(u => u.Login == user.Login))
				return false;

			//добавление пользователя
			await Users.AddAsync(user);
			await _dbContext.SaveChangesAsync(ct);
			return true;
		}

		public async Task<List<User>> GetAllUsersAsync(CancellationToken ct)
		{
			return await Users.ToListAsync();
		}

		public async Task<User?> GetUserByLoginAsync(string login, CancellationToken ct)
		{
			if (string.IsNullOrWhiteSpace(login)) throw new ArgumentNullException(nameof(login));
			return await Users.FirstOrDefaultAsync(u => u.Login == login);
		}

		public async Task UpdateUserAsync(User user, CancellationToken ct)
		{
			if(user is not null)
			{
				Users.Update(user);
				await _dbContext.SaveChangesAsync(ct);
			}
		}
		public async Task<bool> DeleteUserAsync(User user, CancellationToken ct)
		{
			var _user = await GetUserByLoginAsync(user.Login, ct);
			if (_user is not null)
			{
				Users.Remove(_user);
				await _dbContext.SaveChangesAsync(ct);
				return true;
			}
			return false;
		}
	}
}
