using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIServer.Domain.Entities
{
	public class UserDto
	{
        public string Login { get; set; }
        public string Password { get; set; }

        public UserDto(string login, string password)
        {
            this.Login = login;
            this.Password = password;
        }
    }
}
