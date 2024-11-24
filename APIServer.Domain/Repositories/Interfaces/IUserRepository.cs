using APIServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIServer.Domain.Repositories.Interfaces
{
	public interface IUserRepository : IRepository<User>
	{
		Task<bool> AddUserAsync(User user, CancellationToken ct);
		Task<List<User>> GetAllUsersAsync(CancellationToken ct);
		Task<User?> GetUserByLoginAsync(string login, CancellationToken ct);
		Task UpdateUserAsync(User user, CancellationToken ct);
		Task<bool> DeleteUserAsync(User user, CancellationToken ct);
	}
}
