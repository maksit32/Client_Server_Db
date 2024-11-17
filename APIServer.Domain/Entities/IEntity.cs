using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIServer.Domain.Entities
{
	public interface IEntity
	{
		Guid Id { get; init; }
	}
}
