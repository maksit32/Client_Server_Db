using APIServer.DbContexts;
using APIServer.Domain.Exceptions;
using APIServer.Domain.Repositories.Interfaces;
using APIServer.Domain.Services.Interfaces;
using APIServer.Repositories;
using APIServer.Services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Linq;
using Serilog;
using Serilog.Core;
using System.Text;


namespace APIServer
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			Log.Logger = new LoggerConfiguration()
				.WriteTo.Console()
				.CreateLogger();
			Log.Information("Server is up");
			try
			{
				var builder = WebApplication.CreateBuilder(args);
				builder.Host.UseSerilog((hbc, conf) =>
				{
					conf.MinimumLevel.Information()
						.WriteTo.Console()
						.MinimumLevel.Information();
				});
				builder.Services.AddControllers().AddNewtonsoftJson();

				builder.Services.AddEndpointsApiExplorer();
				builder.Services.AddSwaggerGen(options =>
				{
					options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
					{
						Name = "Authorization",
						Type = SecuritySchemeType.ApiKey,
						Scheme = "Bearer",
						BearerFormat = "JWT",
						In = ParameterLocation.Header,
						Description = "JWT Authorization header using the Bearer scheme. " +
									  "\r\n\r\n Enter 'Bearer' [space] and then your token in the text input below." +
										 "\r\n\r\nExample: \"Bearer {token}\"",
					});
					options.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = "Bearer"
							}
						},
						Array.Empty<string>()
					}
				});
				});

				//из secrets.json автоматом
				builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
				builder.Services.AddDbContext<ServerDbContext>(options =>
					options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreeSqlConnectionString") ?? throw new EmptyValueConnectionStringException("Connection string not found!")));
				builder.Services.AddScoped(typeof(IRepository<>), typeof(EFCoreRepository<>));
				//контракт (DI контейнер)
				builder.Services.AddScoped<IUserRepository, UserRepository>();
				builder.Services.AddScoped<IMessageRepository, MessageRepository>();

				builder.Services.AddScoped<IUserValidationService, UserValidationService>();
				builder.Services.AddScoped<IPasswordHasherService, PasswordHasherService>();
				builder.Services.AddScoped<IMessageHasherService, MessageHasherService>();



				builder.Services.AddCors();
				builder.Services.AddHttpLogging(options =>
				{
					options.LoggingFields = HttpLoggingFields.RequestHeaders
											| HttpLoggingFields.ResponseHeaders
											| HttpLoggingFields.RequestBody
											| HttpLoggingFields.ResponseBody;
				});

				var app = builder.Build();
				app.UseHttpsRedirection();
				app.UseHttpLogging();
				app.UseRouting();
				app.UseCors(policy =>
				{
					policy
						.AllowAnyMethod()
						.WithOrigins("http://localhost:8443", "https://localhost:5173", "http://localhost:5174")
						.AllowAnyHeader()
						.AllowCredentials();
				});
				app.UseAuthentication();
				app.UseAuthorization();
				app.UseSwagger();
				app.UseSwaggerUI(options =>
				{
					options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
				});
				app.MapControllers();
				await app.RunAsync();
			}
			catch (Exception ex)
			{
				Log.Fatal(ex, "Unexpected error");
			}
			finally
			{
				Log.Information("Server shutting down");
				await Log.CloseAndFlushAsync();
			}
		}
	}
}