using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIServer.Domain.Entities
{
	[Serializable]
	public class User : IEntity
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; init; }
        public string Login { get; set; }
		public string HashedPassword { get; set; }

		//fluent API
		public List<Message> Messages { get; set; } = new();

		protected User() { }

		public User(string login, string hashedPassword)
		{
			Login = login;
			HashedPassword = hashedPassword;
		}

		public override string ToString()
		{
			return $"{Login}	{HashedPassword}";
		}
	}
}
