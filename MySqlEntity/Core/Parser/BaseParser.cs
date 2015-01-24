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
		public BaseParser (string dbName)
		{
		}
		/// <summary>
		/// Gets the table.
		/// </summary>
		/// <returns>A bunch of tables wich can be executed by sql</returns>
		/// <param name="obj">Object. Any class with properties</param>
		public List<Table> getTable (object obj,string databasename)
		{
			this.ClassObject = obj;
			this.mDbname = databasename;
			return GenerateTable ();

		}


		/// <summary>
		/// Generates the table.
		/// </summary>
		/// <returns>A List with Table(s)</returns>
		private List<Table> GenerateTable()
		{

			List<Table> tables = new List<Table> ();

			Type classType = this.ClassObject.GetType ();

			//Get all Properties from object
			PropertyInfo[] infos = classType.GetProperties(BindingFlags.Public|BindingFlags.Instance);

			//Nessary for the table
			List<Property> properties = new List<Property> ();
			Table table = new Table ();

			foreach (var info in infos) {

				var property = GetProperty (info);

				var attributes = info.GetCustomAttributes (false);
				
				foreach (var atr in attributes) {
					if (atr.GetType () == typeof(IDKey)) { // looks like Id
						IDKey key = (IDKey)atr;
						if (key.isAutoincrement) { //Check if Autoincrement
							table.AUTOINCREMENT = true;
						}
						table.PRIMARYKEY = property;
						property.AttributeTyp = AttributeTyp.Key;
					} else if (atr.GetType () == typeof(PrimitivList)) {
						PrimitivList primitivListAttribute = (PrimitivList)atr;

						string primitivListname = primitivListAttribute.Name; //Name of the list must be unique otherwise it will be overridden

						tables.Add (GenerateList (property, primitivListname));

						property = new Property (primitivListname, typeof(string), primitivListname,AttributeTyp.PrimitvList); // Generate the Foreignkey to the List
					} else if (atr.GetType () == typeof(ForeignKey)) {
						ForeignKey fKey = (ForeignKey)atr;
						property.ForeignType = fKey.ForeignKeyType;
						property.ValueType = typeof(int);
						int id = 0;
						if (property.Value != null) {
							id = ((IEntity)property.Value).GetId ();
							property.Value = id;

						}
						property.AttributeTyp = AttributeTyp.Foreignkey;
					}
				}

				properties.Add (property);

			}
			table.Properties = properties;
			table.TableName = this.ClassObject.GetType ().Name;
			table.OriginalObject = this.ClassObject;
			table.DatabaseName = mDbname;
			tables.Add (table);
			return tables;
		}

		private Table GenerateList(Property property,string name)
		{
			List<object> list = (List<object>)property.Value;
			return GenerateTableFromList<object> (list, name, mDbname);
		}

		public Table GenerateTableFromList<TEntity>(List<TEntity> list,string name,string databasename)
		{
			Table table = new Table ();
			table.TableName = name;
			table.State = ETableState.PrimitivList;

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
		public string mDbname{ get; set; }
	}
}

