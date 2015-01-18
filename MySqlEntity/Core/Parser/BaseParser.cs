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
			Table table = new Table ();//this.ClassObject.GetType ().Name, properties);

			foreach (var info in infos) {

				var property = GetProperty (info);

				var attributes = info.GetCustomAttributes (false);
				
				foreach (var atr in attributes) {
					if (atr.GetType() == typeof(IDKey)) {
						IDKey key = (IDKey)atr;
						if (key.isAutoincrement) {
							table.AUTOINCREMENT = true;
						}
						table.PRIMARYKEY = property;
					}
				}

				properties.Add (property);

			}
			table.Properties = properties;
			table.TableName = this.ClassObject.GetType ().Name;
			table.OriginalObject = this.ClassObject;
			table.DatabaseName = mDbname;
			return table;
		}

		public Table GenerateTableFromList<TEntity>(List<TEntity> list,string name,string databasename)
		{
			Table table = new Table ();
			table.TableName = name;

			var properties = new List<Property> ();
			foreach (var item in list) {
				string columnName = name + (list.IndexOf (item).ToString());
				properties.Add (new Property(columnName,item.GetType(),item));
			}
			table.Properties = properties;
			table.DatabaseName = databasename;
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

