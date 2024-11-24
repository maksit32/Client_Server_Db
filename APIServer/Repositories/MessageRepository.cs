using APIServer.DbContexts;
using APIServer.Domain.Entities;
using APIServer.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APIServer.Repositories
{
	public class MessageRepository : IMessageRepository
	{
		private readonly ServerDbContext _dbContext;
		private DbSet<Message> Messages => _dbContext.Set<Message>();

		public MessageRepository(ServerDbContext dbContext)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
		}

		public async Task<bool> AddMessageAsync(Message message, CancellationToken ct)
		{
			if (message is null) throw new ArgumentNullException(nameof(message));
			await Messages.AddAsync(message, ct);
			await _dbContext.SaveChangesAsync(ct);
			return true;
		}
		public async Task<Message?> GetMessageByTextAsync(string hashedText, CancellationToken ct)
		{
			if (string.IsNullOrWhiteSpace(hashedText)) throw new ArgumentNullException(nameof(hashedText));
			return await Messages.FirstOrDefaultAsync(m => m.HashedText == hashedText);
		}

		public async Task<List<Message>> GetAllMessagesAsync(CancellationToken ct)
		{
			return await Messages.ToListAsync(ct);
		}

		public async Task UpdateMessageAsync(Message message, CancellationToken ct)
		{
			if (message is null) throw new ArgumentNullException(nameof(message));
			Messages.Update(message);
			await _dbContext.SaveChangesAsync(ct);
		}

		public async Task UpdateMessageAsync(Message message, string newHashedText, CancellationToken ct)
		{
			if (message is null) throw new ArgumentNullException(nameof(message));
			if(string.IsNullOrWhiteSpace(newHashedText)) throw new ArgumentNullException(nameof(newHashedText));

			message.HashedText = newHashedText;
			Messages.Update(message);
			await _dbContext.SaveChangesAsync(ct);
		}

		public async Task DeleteMessageAsync(Message message, CancellationToken ct)
		{
			Messages.Remove(message);
			await _dbContext.SaveChangesAsync(ct);
		}

		public async Task DeleteMessageAsync(string hashedText, CancellationToken ct)
		{
			var message = await GetMessageByTextAsync(hashedText, ct);
			await DeleteMessageAsync(message, ct);
		}
	}
}
