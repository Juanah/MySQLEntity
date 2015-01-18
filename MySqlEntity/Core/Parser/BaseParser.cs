using System;
using Infrastructure.Core;
using System.Reflection;
using System.Reflection.Emit;
using Common;
using System.Collections.Generic;
using System.Linq;
using Infrastructure;

namespace Core
{
	/// <summary>
	/// BasePaser who is able to convert Objects into Table objects
	/// </summary>
	public class BaseParser:IClassParser
	{
		public BaseParser ()
		{
		}

		/// <summary>
		/// Gets the table.
		/// </summary>
		/// <returns>The table.</returns>
		/// <param name="obj">Object. Any class with properties</param>
		public Table getTable (object obj,string databasename)
		{
			this.ClassObject = obj;
			this.mDbname = databasename;
			return GenerateTable ();

		}

		/// <summary>
		/// Generates the table.
		/// </summary>
		/// <returns>The table.</returns>
		private Table GenerateTable()
		{
			Type classType = this.ClassObject.GetType ();

			PropertyInfo[] infos = classType.GetProperties(BindingFlags.Public|BindingFlags.Instance);

			List<Property> properties = new List<Property> ();
			foreach (var info in infos) {
				properties.Add (GetProperty (info));

				var attributes = info.GetCustomAttributes (false);

				foreach (var atr in attributes) {
					Console.WriteLine (atr.ToString());
				}



			}
			Table table = new Table (this.ClassObject.GetType ().Name, properties);
			table.OriginalObject = this.ClassObject;
			table.PRIMARYKEY = table.Properties [0];
			table.DatabaseName = mDbname;
			return table;
		}

		/// <summary>
		/// Gets the property.
		/// </summary>
		/// <returns>The property.</returns>
		/// <param name="info">Infos about name value and valuetype</param>
		private Property GetProperty(PropertyInfo info)
		{
			Property property = new Property () {
				PropertyName = info.Name,
				ValueType = info.PropertyType,
				Value = info.GetValue(this.ClassObject,null)
			};
			return property;
		}

		private Object ClassObject;
		private string mDbname;
	}
}

