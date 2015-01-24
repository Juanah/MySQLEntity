using System;
using Infrastructure;
using System.Collections.Generic;
using System.Reflection;
using Common;
using System.Linq;


namespace Core
{
	/// <summary>
	/// Decodes the given SQLData in one Class Object
	/// </summary>
	public class BaseDecoder: IDecoder
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Core.BaseDecoder"/> class.
		/// </summary>
		public BaseDecoder ()
		{
		}

		#region IDecoder implementation

		/// <summary>
		/// Decodes a given object list into the parent and returns it
		/// </summary>
		/// <param name="objects">SQldatabase objects</param>
		/// <param name="parent">EntityBaseClass</param>
		public object Decode (List<Object> objects,Table table,object context)
		{
			object origClone = ((IEntity)table.OriginalObject).DeepCopy ();
			var counter = 0;
			foreach (var property in table.Properties) {
				PropertyInfo prop = origClone.GetType().GetProperty(property.PropertyName, BindingFlags.Public | BindingFlags.Instance);
				object obj = objects [counter];
				if (property.AttributeTyp == AttributeTyp.Foreignkey) {
					int id = (int)objects[counter];
					obj = (object)((Context)context).GetTable<IEntity> (property.ForeignType).FirstOrDefault(i => i.GetId().Equals(id));
				} 
				if (null != prop && prop.CanWrite) {
					prop.SetValue (origClone, obj);
				}
				counter++;
			}
			return origClone;
		}
		#endregion
	}
}


