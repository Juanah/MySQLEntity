using System;
using Infrastructure;

namespace MySqlEntityTest
{
	public class TestEntity:IEntity
	{
		public TestEntity ()
		{
		}

		public int ID{ get; set; }

		public string Name{ get; set; }

		#region IEntity implementation

		public object DeepCopy ()
		{

			TestEntity other = (TestEntity) this.MemberwiseClone(); 
			return other;

		}

		#endregion
	}
}

