using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIServer.Domain.Services.Interfaces
{
	public interface IPasswordHasherService
	{
		string HashPassword(string password);
		bool VerifyPassword(string hashedPassword, string notHashedPassword);
	}
}
