using System;
using Infrastructure;

namespace MySqlEntityTest
{
	public class TestEntity2:IEntity
	{
		public TestEntity2 ()
		{
		}

		[IDKey(true)]
		public int Id{get;set;}


		public string Haus{get;set;}

		#region IEntity implementation

		public object DeepCopy ()
		{
			return (TestEntity2)this.MemberwiseClone ();
		}

		#endregion
	}
}

