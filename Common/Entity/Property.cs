using System;

namespace Common
{
	/// <summary>
	/// Property class is an important Class 
	/// wich stores name,values and types
	/// </summary>
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

		/// <summary>
		/// Gets or sets the name of the property.
		/// </summary>
		/// <value>The name of the property.</value>
		public string PropertyName{get;set;}

		/// <summary>
		/// Gets or sets the type of the value.
		/// </summary>
		/// <value>The type of the value.</value>
		public Type ValueType{ get; set;}

		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		/// <value>The value.</value>
		public Object Value{get;set;}

		/// <summary>
		/// Gets or sets the attribute typ.
		/// </summary>
		/// <value>The attribute typ.</value>
		public AttributeTyp AttributeTyp{ get; set;}

		/// <summary>
		/// Gets or sets the type of the foreign.
		/// </summary>
		/// <value>The type of the foreign.</value>
		public Type ForeignType{get;set;}

		/// <summary>
		/// Gets or sets the name of the foreign object.
		/// </summary>
		/// <value>The name of the foreign object.</value>
		public string ForeignObjectName{get;set;}

	}
}

