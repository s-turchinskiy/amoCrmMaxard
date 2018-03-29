using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmoCRM.Models
{
	public class UserResponse
	{
		public string id { get; set; }
		public string name { get; set; }
		public string last_name { get; set; }

	}

	public class Leads_statusesResponse
	{
		public string id { get; set; }
		public string name { get; set; }
		public string pipeline_id { get; set; }
		public string sort { get; set; }

	}

	public class AccountResponseCurrent
	{
		public string id { get; set; }
		public string name { get; set; }
		public List<UserResponse> users { get; set; }
		public List<Leads_statusesResponse> leads_statuses { get; set; }

	}

	public class AccountsResponseCurrent
	{
		public AccountResponseCurrent account { get; set; }
		public int server_time { get; set; }
	}

	public class AccountResponseCurrentRoot
	{
		public AccountsResponseCurrent response { get; set; }
	}
}
