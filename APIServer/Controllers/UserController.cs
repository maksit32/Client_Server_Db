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
	public class UserController : ControllerBase
	{
		//services
		private readonly IPasswordHasherService _passwordHasherService;
		private readonly IUserCreatorService _userCreatorService;

		//repositories
		private readonly IUserRepository _userRepo;

		//constructors
		public UserController(IPasswordHasherService passwordHasherService, IUserCreatorService userCreatorService,
			IUserRepository userRepo)
		{
			_passwordHasherService = passwordHasherService ?? throw new ArgumentNullException(nameof(passwordHasherService), "Password hasher service is null!");
			_userCreatorService = userCreatorService ?? throw new ArgumentNullException(nameof(userCreatorService), "User creator service is null!");
			_userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo), "User repositpry is null!");
		}

		//methods
		[HttpPost("register")]
		public async Task<ActionResult> Register([FromBody] UserDto userDto, CancellationToken ct)
		{
			try
			{
				var hashedPassword = _passwordHasherService.HashPassword(userDto.Password);
				var user = _userCreatorService.CreateUser(userDto.Login, hashedPassword);
				var res = await _userRepo.AddUserAsync(user, ct);
				if (!res) return Unauthorized();

				return Ok();
			}
			catch (Exception)
			{
				return Unauthorized();
			}
		}
		[HttpPost("login")]
		public async Task<ActionResult> Login([FromBody] UserDto user, CancellationToken ct)
		{
			try
			{
				var dbUser = await _userRepo.GetUserByLoginAsync(user.Login, ct);
				var res = _passwordHasherService.VerifyPassword(dbUser.HashedPassword, user.Password);
				if (!res) return Unauthorized();
				return Ok();
			}
			catch (Exception)
			{
				return Unauthorized();
			}
		}
		[HttpGet("get_users")]
		public async Task<ActionResult<List<User>>> GetUsers(CancellationToken ct)
		{
			try
			{
				return await _userRepo.GetAllUsersAsync(ct);
			}
			catch (Exception)
			{
				return BadRequest();
			}
		}
		[HttpGet("get")]
		public async Task<ActionResult<User?>> GetUserByLogin([FromQuery] string login, CancellationToken ct)
		{
			try
			{
				return await _userRepo.GetUserByLoginAsync(login, ct);
			}
			catch (Exception)
			{
				return BadRequest();
			}
		}
		[HttpPost("update")]
		public async Task<ActionResult> Update([FromBody] UserDto user, CancellationToken ct)
		{
			try
			{
				var dbUser = await _userRepo.GetUserByLoginAsync(user.Login, ct);
				var newHashedPassword = _passwordHasherService.HashPassword(user.Password);
				var newUser = _userCreatorService.UpdateUser(dbUser, newHashedPassword);
				await _userRepo.UpdateUserAsync(newUser, ct);

				return Ok();
			}
			catch (Exception)
			{
				return BadRequest();
			}
		}
		[HttpDelete("delete")]
		public async Task<ActionResult> Delete([FromQuery] string login, CancellationToken ct)
		{
			try
			{
				var dbUser = await _userRepo.GetUserByLoginAsync(login, ct);
				var res = await _userRepo.DeleteUserAsync(dbUser, ct);
				if(!res) return BadRequest();

				return Ok();
			}
			catch (Exception)
			{
				return BadRequest();
			}
		}
	}
}
