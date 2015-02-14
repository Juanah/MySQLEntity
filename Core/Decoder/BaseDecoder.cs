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
				//if(property.AttributeTyp == AttributeTyp.
				if (null != prop && prop.CanWrite) {
					prop.SetValue (origClone, obj);
				}
				counter++;
			}
			return origClone;
		}

		public object GetList (string idList,object context)
		{
			var typeString = idList.Substring (0, idList.IndexOf ('/'));

			idList.Replace (typeString + "/", "");
			IEntity entity = ((Context)context).Entities.FirstOrDefault (e => e.GetType ().ToString ().Equals (typeString));
			//((Context)context).GetTable (typeof(entity));
			if (entity == null) {
				throw new ArgumentNullException ("Entity is null");
			}

			var ids = idList.Split ('-');
			List<int> iId = new List<int> ();
			foreach (var item in ids) {
				iId.Add (Convert.ToInt32(item));
			}

			List<IEntity> entities = new List<IEntity> ();

			return null;

		}

		#endregion
	}
}


