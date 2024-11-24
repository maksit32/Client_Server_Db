using APIServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIServer.Domain.Repositories.Interfaces
{
	public interface IMessageRepository : IRepository<Message>
	{
		Task<bool> AddMessageAsync(Message message, CancellationToken ct);
		Task<List<Message>> GetAllMessagesAsync(CancellationToken ct);
		Task<Message?> GetMessageByTextAsync(string hashedText, CancellationToken ct);
		Task UpdateMessageAsync(Message message, CancellationToken ct);
		Task UpdateMessageAsync(Message message, string newHashedText, CancellationToken ct);
		Task DeleteMessageAsync(Message message, CancellationToken ct);
		Task DeleteMessageAsync(string hashedText, CancellationToken ct);
	}
}
