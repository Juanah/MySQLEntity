using System;
using Infrastructure;
using System.Collections.Generic;
using System.Reflection;
using Common;


namespace Core
{
	public class BaseDecoder: IDecoder
	{
		private static BaseParser mParser = new BaseParser();
		public BaseDecoder ()
		{
		}

		#region IDecoder implementation
		public object Decode (List<Object> objects,Object parent)
		{
			Table table = mParser.getTable (parent,"");
			var counter = 0;
			foreach (var item in table.Properties) {
				PropertyInfo prop = parent.GetType().GetProperty(item.PropertyName, BindingFlags.Public | BindingFlags.Instance);
				if (null != prop && prop.CanWrite) {
					prop.SetValue (parent, objects [counter]);
				}
				counter++;
			}
			return parent;
		}
		#endregion
	}
}


