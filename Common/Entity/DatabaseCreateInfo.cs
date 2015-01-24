using System;
using System.Collections.Generic;

namespace Common
{
	/// <summary>
	/// Database create info.
	/// Stores the Classes wich will be the Dbmodel
	/// </summary>
	public class DatabaseCreateInfo
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Common.DatabaseCreateInfo"/> class.
		/// </summary>
		public DatabaseCreateInfo ()
		{
			Classes = new List<object> ();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Common.DatabaseCreateInfo"/> class.
		/// </summary>
		/// <param name="classes">Classes.</param>
		public DatabaseCreateInfo (IList<object> classes)
		{
			this.Classes = classes;
		}
		
		/// <summary>
		/// Classes wich will be a part of the Databasemodel
		/// </summary>
		/// <value>The classes.</value>
		public IList<Object> Classes{ get; set;}
	}
}

