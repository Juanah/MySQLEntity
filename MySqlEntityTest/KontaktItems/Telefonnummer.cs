using System;
using Infrastructure;

namespace MySqlEntityTest
{
	public class Telefonnummer: IEntity 
	{
		public Telefonnummer ()
		{
		}
		[IDKey(true)]
		public int Id{ get; set;}

		public string Tel{ get; set;}

		#region IEntity implementation

		public object DeepCopy ()
		{
			return this.MemberwiseClone ();
		}

		public int GetId ()
		{
			return Id;
		}

		#endregion
	}
}

