using System;

namespace Common
{
	public class Property: Table
	{
		public Property ()
		{
		}

		[Obsolete("the type should be passed")]
		public Property (string propertyName, System.Type valueType, object value)
		{
			this.PropertyName = propertyName;
			this.ValueType = valueType;
			this.Value = value;
		}

		public Property (string propertyName, System.Type valueType, object value, Common.AttributeTyp attributeTyp)
		{
			this.PropertyName = propertyName;
			this.ValueType = valueType;
			this.Value = value;
			this.AttributeTyp = attributeTyp;
		}
		
		public string PropertyName{get;set;}

		public Type ValueType{ get; set;}

		public Object Value{get;set;}

		public AttributeTyp AttributeTyp{ get; set;}

		public Type ForeignType{get;set;}

		public string ForeignObjectName{get;set;}

	}
}

