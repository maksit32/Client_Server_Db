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

		//repositories
		private readonly IMessageRepository _messageRepo;

		public MessageController(IMessageHasherService messageHasherService, IMessageRepository messageRepo)
		{
			_messageHasherService = messageHasherService ?? throw new ArgumentNullException(nameof(messageHasherService));
			_messageRepo = messageRepo ?? throw new ArgumentNullException(nameof(messageRepo));
		}
		//methods
		[HttpPost("send")] 
	}
}
