using APIServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIServer.Domain.Repositories.Interfaces
{
	//общие для всех репозиториев методы
	public interface IRepository<TEntity> where TEntity : IEntity
	{
		//Task<User?> GetUserByLoginAsync(string login, CancellationToken ct);
	}
}
