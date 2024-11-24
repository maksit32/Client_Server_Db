using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIServer.Domain.Entities
{
	public class MessageDto
	{
        //from user
        public string Login { get; set; }
        public string Password { get; set; }
        public string Message { get; set; }
        public MessageDto(string login, string password, string message)
        {
            this.Login = login;
            this.Password = password;
            this.Message = message;
        }
    }
}
