using System;

namespace ContactManager
{
	public class Contact
	{
		public string name{ get; set; }
		public string phone{ get; set; }




		public Contact (string name, string phone)
		{
			this.name = name;
			this.phone = phone;
		}
	}
}

