using System;

namespace Common
{
	public class DB
	{
		public DB ()
		{
		}

		public DB (string name)
		{
			this.DatabaseName = name;
		}
		

		public string DatabaseName{ get; set; }
	}
}

