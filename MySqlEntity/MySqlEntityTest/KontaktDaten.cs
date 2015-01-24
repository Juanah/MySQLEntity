using System;
using Infrastructure;

namespace MySqlEntityTest
{
	public class KontaktDaten:IEntity 
	{
		public KontaktDaten ()
		{
		}
		[IDKey(true)]
		public int Id{ get; set;}

		[ForeignKey(typeof(Telefonnummer))]
		public Telefonnummer Telefonnummer{ get; set;}


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

