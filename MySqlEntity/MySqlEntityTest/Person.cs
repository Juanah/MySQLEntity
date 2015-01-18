using System;
using Infrastructure;

namespace MySqlEntityTest
{
	public class Person:IEntity
	{
		public Person ()
		{
		}

		[IDKey(true)]
		public int Id{ get; set; }

		public string Name{ get; set; }

		public string Nachname{ get; set; }

		public double Gehalt{ get; set; }

		#region IEntity implementation
		public object DeepCopy ()
		{
			return (Person)this.MemberwiseClone ();
		}
		#endregion
	}
}

