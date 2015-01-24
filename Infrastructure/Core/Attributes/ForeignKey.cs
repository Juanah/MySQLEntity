using System;

namespace Infrastructure
{
	[AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
	public class ForeignKey: Attribute
	{
		public ForeignKey (System.Type foreignKeyType)
		{
			this.ForeignKeyType = foreignKeyType;
		}
		
		public Type ForeignKeyType{ get; set; }
	}
}

