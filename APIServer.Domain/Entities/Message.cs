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
	public class Message : IEntity
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; init; }
		public string HashedText { get; set; }
        public User User { get; set; }

        protected Message() { }
		public Message(string hashedText, User user)
		{
			HashedText = hashedText;
			User = user;
		}
		public override string ToString()
		{
			return $"{HashedText}\n";
		}
	}
}
