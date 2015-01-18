using System;
using Infrastructure.Core;
using Common;

namespace Core
{
	public class CreateParser: IParser 
	{
		public CreateParser ()
		{
		}

		public Common.SqlQuery getSQLQuery(Table table)
		{
			SqlQuery query = new SqlQuery (GetTableStr (table),false);
			return query;
		}


		private string GetTableStr(Table table)
		{

			char up = '`';

			string tableQuery = "CREATE TABLE ";
			tableQuery += '`' + table.DatabaseName + '`' + ".`" + table.TableName + '`'
			+ "(";

			var counter = 0;

			foreach (var property in table.Properties) {

				if (property == table.PRIMARYKEY) {
			
					tableQuery += " " + up 
						+ property.PropertyName + up
						+ " " + getType (property) + " NOT NULL";
					if (table.AUTOINCREMENT) {
						tableQuery += " AUTO_INCREMENT,";
					} else {
						tableQuery += ",";
					}
				} else {
					tableQuery += " " + up 
						+ property.PropertyName + up
							+ " " + getType(property) + " NULL,";
				}
				counter++;
			}

			if (table.PRIMARYKEY != null) {
				tableQuery += " PRIMARY KEY (" + up + table.PRIMARYKEY.PropertyName + up + "));";
			} else {
				tableQuery += ")";
			}
			Console.WriteLine (tableQuery);
			return tableQuery;
		}

		private string getType(Property property)
		{

			Type p = property.ValueType;

			Type s = typeof(string);
			Type i = typeof(int);
			Type d = typeof(double);
			Type f = typeof(float);


			if (p == s) {
				return "VARCHAR(1000)";
			} else if (p == i) {
				return "INT";
			} else if (p == d) {
				return "DOUBLE";
			} else if (p == f) {
				return "FLOAT";
			} else {
				return "";
			}

		}

	}
}

