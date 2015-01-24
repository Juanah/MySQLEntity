using System;
using Infrastructure.Core;
using Common;

namespace Core
{
	/// <summary>
	/// Creates Table and Database Create sql queries
	/// </summary>
	public class CreateParser: IParser 
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Core.CreateParser"/> class.
		/// </summary>
		public CreateParser ()
		{
		}
		/// <summary>
		/// Gets the SQL query.
		/// </summary>
		/// <returns>The SQL query.</returns>
		/// <param name="table">Table.</param>
		public Common.SqlQuery getSQLQuery(Table table)
		{
			SqlQuery query = new SqlQuery (GetTableStr (table),false);
			return query;
		}

		/// <summary>
		/// Gets the table string.
		/// </summary>
		/// <returns>The table string.</returns>
		/// <param name="table">Table.</param>
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
						if (table.Properties.IndexOf (property) == (table.Properties.Count - 1)) {
							tableQuery += " AUTO_INCREMENT,";
						} else {
							tableQuery += " AUTO_INCREMENT,";
						}
					} else {
						tableQuery += ",";
					}
				} else {
					tableQuery += " " + up 
						+ property.PropertyName + up
						+ " " + getType (property);

					if (table.Properties.IndexOf (property) == (table.Properties.Count - 1)) {
						if (table.PRIMARYKEY != null) {
							tableQuery += " NULL,";
						} else {
							tableQuery += " NULL)";
						}
					} else {
						tableQuery += " NULL,";
					}
				}
				counter++;
			}
			if (table.PRIMARYKEY != null) {
				tableQuery += " PRIMARY KEY (" + up + table.PRIMARYKEY.PropertyName + up + "));";
			} else {
				//tableQuery += ")";
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

