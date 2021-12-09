using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities
{
	public class User
	{
		public int Id { get; set; }

		public string Email { get; set; }

		public string Username { get; set; }

		public string Password { get; set; }

		public string Salt { get; set; }

		public string FirstName { get; set; }

		public string MiddleName { get; set; }

		public string LastName { get; set; }

		public DateTime Birthday { get; set; }

		public string Phone { get; set; }

		public DateTime RegisterDate { get; set; }
	}
}
