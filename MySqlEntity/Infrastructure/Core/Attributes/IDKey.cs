using System;

namespace Infrastructure
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class IDKey: Attribute
	{
		public IDKey (bool autoincrement)
		{
		}
	}
}

