using System;

namespace Common
{
	[Obsolete("Wird die noch verwendet?")]
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

