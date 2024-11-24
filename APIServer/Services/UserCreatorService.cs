using APIServer.Domain.Entities;
using APIServer.Domain.Services.Interfaces;

namespace APIServer.Services
{
	public class UserCreatorService : IUserCreatorService
	{
		public User CreateUser(string login, string hashedPassword)
		{
			return new User(login, hashedPassword);
		}
		public User UpdateUser(User user, string hashedPassword)
		{
			user.HashedPassword = hashedPassword;
			return user;
		}
	}
}
