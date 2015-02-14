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

		private IDBConnectionInfo _connectionInfo;

		public BaseParser (IDBConnectionInfo _connectionInfo)
		{
			this._connectionInfo = _connectionInfo;
		}

		/// <summary>
		/// Gets the table.
		/// </summary>
		/// <returns>A bunch of tables wich can be executed by sql</returns>
		/// <param name="obj">Object. Any class with properties</param>
		[Obsolete("cause Unity")]
		public List<Table> getTable (object obj,string databasename)
		{
			this.ClassObject = obj;
			this.mDbname = databasename;
			return GenerateTable ();
		}

		/// <summary>
		/// Gets the tables.
		/// </summary>
		/// <returns>The table.</returns>
		/// <param name="obj">Object.</param>
		public List<Table> getTable (object obj)
		{
			this.ClassObject = obj;
			this.mDbname = _connectionInfo.GetDatabasename();
			return GenerateTable ();
		}

		/// <summary>
		/// Generates the table.
		/// </summary>
		/// <returns>A List with Table(s)</returns>
		private List<Table> GenerateTable()
		{
			List<Table> tables = new List<Table> (); //Will be returned
			Type classType = this.ClassObject.GetType (); //Type from baseClass
			//Get all Properties from object
			PropertyInfo[] infos = classType.GetProperties(BindingFlags.Public|BindingFlags.Instance);
			//Nessary for the table
			List<Property> properties = new List<Property> ();
			Table table = new Table (); //State Normal, it is the Base
			table.State = ETableState.Normal;
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

						property = new Property (primitivListname, typeof(string), primitivListname, AttributeTyp.PrimitvList); // Generate the Foreignkey to the List
					} else if (atr.GetType () == typeof(ForeignKey)) {
						ForeignKey fKey = (ForeignKey)atr;
						property.ForeignType = fKey.ForeignKeyType;
						property.ValueType = typeof(int); //Instead of the real neasted object, we will put the reference Id as integer
						int id = 0;
						if (property.Value != null) {
							id = ((IEntity)property.Value).GetId ();
							if (id == 0) {
								Context.Insert (((IEntity)property.Value));
								id = ((IEntity)property.Value).GetId ();
							}
							property.Value = id;
						}
						property.AttributeTyp = AttributeTyp.Foreignkey;
					} else if (atr.GetType () == typeof(Foreignkeylist)) {
						if (this.Context == null) {
							throw new NullReferenceException ("Context is null");
						}

						Foreignkeylist foreignkeyListAtr = (Foreignkeylist)atr;

						List<int> tempIds = new List<int> ();
						//Cast property to List
						List<IEntity> tempList = (List<IEntity>)property.Value;
						foreach (var item in tempList) {
							Context.Insert (item);
							tempIds.Add (item.GetId());
						}

						//Build string with IDs and type wich will be inserted
						string tempIdList = property.ValueType.ToString () + "/";
						foreach (var id in tempIds) {
							tempIdList += id + "-";
						}

						property.ValueType = typeof(string);
						property.Value = tempIdList;

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

		/// <summary>
		/// Experimental
		/// shoud convert PrimitvTyp Lists into a table
		/// </summary>
		/// <returns>The table from list.</returns>
		/// <param name="list">List.</param>
		/// <param name="name">Name.</param>
		/// <param name="databasename">Databasename.</param>
		/// <typeparam name="TEntity">The 1st type parameter.</typeparam>
		public Table GenerateTableFromList<TEntity>(List<TEntity> list,string name,string databasename)
		{
			Table table = new Table ();
			table.TableName = name;
			table.State = ETableState.PrimitivList;

			var properties = new List<Property> ();
			foreach (var item in list) {
				string columnName = name + (list.IndexOf (item).ToString());
				properties.Add (new Property(columnName,item.GetType(),item,AttributeTyp.PrimitvList));
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
		/// <summary>
		/// Gets or sets the m dbname.
		/// </summary>
		/// <value>The m dbname.</value>
		public string mDbname{ get; set; }
		public Context Context{ get; set; }
	}
}

