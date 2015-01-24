using System;

namespace Infrastructure
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
	public class PrimitivList:Attribute
	{
		public PrimitivList (string nameUnique)
		{
			this.Name = nameUnique;
		}
		public string Name{ get; private set;}
	}
}

