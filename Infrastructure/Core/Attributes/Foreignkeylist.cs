using System;

namespace Infrastructure
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class Foreignkeylist:Attribute
	{
		public Foreignkeylist (Type entitytype)
		{
			EntityType = entitytype;
		}
		public Type EntityType{get;set;}

	}
}

