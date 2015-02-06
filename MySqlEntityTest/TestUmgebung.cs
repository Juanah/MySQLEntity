using System;
using System.Reflection;

namespace DITest
{
	public class TestUmgebung
	{
		Func<object,bool> isHallo = (entity) =>
		{

			Type eType = entity.GetType();

			PropertyInfo[] infos = eType.GetProperties();

			if(eType.Name == "Hallo")return true;
			return false;
		};

		public int Hallo = 1;

		public TestUmgebung ()
		{



		}

		public void doit()
		{
			TestUmgebung u = new TestUmgebung ();

			var result = u.isHallo (Hallo);

		}

	}
}

