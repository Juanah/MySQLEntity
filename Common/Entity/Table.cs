using System;
using System.Collections.Generic;

namespace Common
{
	/// <summary>
	/// Can be the abstract or absolute mask of the Entity
	/// It is an important part of the Framework
	/// </summary>
	public class Table:DB
	{
		public Table ()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Common.Table"/> class.
		/// </summary>
		/// <param name="tableName">Table name.</param>
		public Table (string tableName)
		{
			this.TableName = tableName;
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Common.Table"/> class.
		/// </summary>
		/// <param name="tableName">Table name.</param>
		/// <param name="properties">Properties.</param>
		public Table (string tableName, List<Property> properties)
		{
			this.TableName = tableName;
			this.Properties = properties;
		}
		
		/// <summary>
		/// Gets or sets the name of the table.
		/// </summary>
		/// <value>The name of the table.</value>
		public string TableName{ get; set; }
		/// <summary>
		/// Property wich represents the PrimaryKey
		/// there can be no PrimaryKey but not 2!
		/// </summary>
		/// <value>The PRIMARYKE.</value>
		public Property PRIMARYKEY{ get; set; }
		/// <summary>
		/// Specify if the Primarykey is autoincrement (raw SQLProperty)
		/// </summary>
		/// <value><c>true</c> if AUTOINCREMEN; otherwise, <c>false</c>.</value>
		public bool AUTOINCREMENT{ get; set; }
		/// <summary>
		/// List of properties wich belong to the table.
		/// each property defines a Sqlcolumn and its value
		/// </summary>
		/// <value>The properties.</value>
		public List<Property> Properties{ get; set; }
		/// <summary>
		/// OriginalEntity Object for cloning
		/// </summary>
		/// <value>The original object.</value>
		public Object OriginalObject{ get; set; }
		/// <summary>
		/// Defines the differnt types of Tables
		/// </summary>
		/// <value>The state.</value>
		public ETableState State{ get; set; }
	}
}

