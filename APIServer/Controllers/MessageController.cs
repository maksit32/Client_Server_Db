using APIServer.Domain.Entities;
using APIServer.Domain.Repositories.Interfaces;
using APIServer.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIServer.Controllers
{
	[ApiController]
	[AllowAnonymous]
	[Route("api/[controller]")]
	public class MessageController : ControllerBase
	{
		//services
		private readonly IMessageHasherService _messageHasherService;
		private readonly IMessageCreatorService _messageCreatorService;

		//repositories
		private readonly IMessageRepository _messageRepo;
		private readonly IUserRepository _userRepository;

		public MessageController(IMessageHasherService messageHasherService, IMessageRepository messageRepo,
									IUserRepository userRepository, IMessageCreatorService messageCreatorService)
		{
			_messageHasherService = messageHasherService ?? throw new ArgumentNullException(nameof(messageHasherService));
			_messageRepo = messageRepo ?? throw new ArgumentNullException(nameof(messageRepo));
			_messageCreatorService = messageCreatorService ?? throw new ArgumentNullException(nameof(messageCreatorService));
			_userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
		}
		//methods
		[HttpPost("send")]
		public async Task<ActionResult> ReceiveMessage([FromBody] MessageDto message, CancellationToken ct)
		{
			try
			{
				var sender = await _userRepository.GetUserByLoginAsync(message.Login, ct);
				var encrMess = _messageHasherService.Encrypt(message.Message);
				var newMess = _messageCreatorService.CreateMessage(encrMess, sender);
				await _messageRepo.AddMessageAsync(newMess, ct);
				return Ok();
			}
			catch (Exception)
			{
				return BadRequest();
			}
		}
		[HttpGet("get_messages")]
		public async Task<ActionResult<List<string>>> GetMessages(CancellationToken ct)
		{
			try
			{
				var encrLst = await _messageRepo.GetAllMessagesAsync(ct);
				var decrLst = encrLst.Select(e => _messageHasherService.Decrypt(e.HashedText)).ToList();
				return decrLst;
			}
			catch (Exception)
			{
				return BadRequest();
			}
		}
		//бред....
		[HttpGet("get_user_message")]
		public async Task<ActionResult<Message>> GetUserMessage([FromQuery] string text, CancellationToken ct)
		{
			try
			{
				var encrMessage = _messageHasherService.Encrypt(text);
				return await _messageRepo.GetMessageByTextAsync(encrMessage, ct);
			}
			catch (Exception)
			{
				return BadRequest();
			}
		}
		[HttpPut("update")]
		public async Task<ActionResult> Update([FromQuery] string newText, [FromBody] MessageDto message, CancellationToken ct)
		{
			try
			{
				var oldHashedText = _messageHasherService.Encrypt(message.Message);
				var oldMess = await _messageRepo.GetMessageByTextAsync(oldHashedText, ct);
				var newHashedText = _messageHasherService.Encrypt(newText);
				await _messageRepo.UpdateMessageAsync(oldMess, newHashedText, ct);
				return Ok();
			}
			catch (Exception)
			{
				return BadRequest();
			}
		}
		[HttpDelete("delete")]
		public async Task<ActionResult> Delete([FromQuery] string text, CancellationToken ct)
		{
			try
			{
				var hashedText = _messageHasherService.Encrypt(text);
				var message = await _messageRepo.GetMessageByTextAsync(hashedText, ct);
				await _messageRepo.DeleteMessageAsync(message, ct);
				return Ok();
			}
			catch (Exception)
			{
				return BadRequest();
			}
		}
	}
}