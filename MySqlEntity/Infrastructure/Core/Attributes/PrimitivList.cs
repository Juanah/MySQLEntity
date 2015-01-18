using System;

namespace Infrastructure
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
	public class PrimitivList:Attribute
	{
		public PrimitivList ()
		{
		}
	}
}

