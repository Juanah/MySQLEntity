using System;
using System.Collections.Generic;

namespace MySqlEntityTest
{
	public class Command
	{
		public Command ()
		{
		}
		public Command (string mainCommand, List<string> attributes, List<string> data)
		{
			this.MainCommand = mainCommand;
			this.Attributes = attributes;
			this.Data = data;
		}
		


		public string MainCommand{ get; set; }
		public List<string> Attributes{ get; set; }
		public List<string> Data{ get; set; }

	}
}

