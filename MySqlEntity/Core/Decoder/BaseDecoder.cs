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
		public BaseDecoder ()
		{
		}

		#region IDecoder implementation

		/// <summary>
		/// Decodes a given object list into the parent and returns him
		/// </summary>
		/// <param name="objects">Objects.</param>
		/// <param name="parent">Parent.</param>
		/// object Decode (List<Object> objects,Object parent,object context);
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



			/*
			//TODO Ist das sicher mit dem leeren parameter ?!
			List<Table> tables = mParser.getTable (parent, "");

			var counter = 0;
			foreach (var table in tables) {
				foreach (var item in table.Properties) {
					PropertyInfo prop = parent.GetType().GetProperty(item.PropertyName, BindingFlags.Public | BindingFlags.Instance);
					if (null != prop && prop.CanWrite) {
						prop.SetValue (parent, objects [counter]);
					}
					counter++;
				}
			}

*/
		}
		#endregion
	}
}


