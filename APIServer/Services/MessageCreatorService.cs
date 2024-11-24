using APIServer.Domain.Entities;
using APIServer.Domain.Services.Interfaces;

namespace APIServer.Services
{
	public class MessageCreatorService : IMessageCreatorService
	{
		public Message CreateMessage(string hashedText, User sender)
		{
			return new Message(hashedText, sender);
		}
	}
}
