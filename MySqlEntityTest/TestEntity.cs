using System;
using Infrastructure;

namespace DITest
{
	public class TestEntity:IEntity
	{
		public TestEntity ()
		{
		}



		public string Name{ get; set; }

		[IDKey(true)]
		public int ID{ get; set; }

		#region IEntity implementation

		public object DeepCopy ()
		{

			TestEntity other = (TestEntity) this.MemberwiseClone(); 
			return other;

		}

		public int GetId ()
		{	
			return ID;
		}

		public void SetID (int id)
		{
			ID = id;
		}

		#endregion
	}
}

