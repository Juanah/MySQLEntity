using System;
using System.Collections.Generic;

namespace Common
{
	public class DatabaseCreateInfo
	{
		public DatabaseCreateInfo ()
		{
			Classes = new List<object> ();
		}

		public DatabaseCreateInfo (IList<object> classes)
		{
			this.Classes = classes;
		}
		

		public IList<Object> Classes{ get; set;}
	}
}

