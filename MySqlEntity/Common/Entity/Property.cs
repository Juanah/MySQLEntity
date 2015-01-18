using System;

namespace Common
{
	public class Property: Table
	{
		public Property ()
		{
		}

		public Property (string propertyName, System.Type valueType, object value)
		{
			this.PropertyName = propertyName;
			this.ValueType = valueType;
			this.Value = value;
		}
		

		public string PropertyName{get;set;}

		public Type ValueType{ get; set;}

		public Object Value{get;set;}

	}
}

