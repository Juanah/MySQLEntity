using System;
using Infrastructure;

namespace MySqlEntityTest
{
	public class ForeignKeyTest: IEntity 
	{
		public ForeignKeyTest ()
		{
		}


		[IDKey(true)]
		public int Id{ get; set;}

		[ForeignKey(typeof(Person))]
		public Person MeineTestPerson{ get; set;}


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

