using System;
using System.Collections.Generic;

namespace Common
{
	public class Table:DB
	{
		public Table ()
		{
		}

		public Table (string tableName)
		{
			this.TableName = tableName;
		}
		

		public Table (string tableName, List<Property> properties)
		{
			this.TableName = tableName;
			this.Properties = properties;
		}
		

		public string TableName{ get; set; }
		public Property PRIMARYKEY{ get; set; }
		public Property AUTOINCREMENT{ get; set; }
		public List<Property> Properties{ get; set; }
		public Object OriginalObject{ get; set; }

	}
}

