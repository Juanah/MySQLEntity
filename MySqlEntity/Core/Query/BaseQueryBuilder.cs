using System;
using Common;
namespace Core
{
	public static class BaseQueryBuilder
	{
		private static char u = '`';
		private static string upKomma = "'";
		/// <summary>
		/// Gets the table query.
		/// </summary>
		/// <returns>The tables.</returns>
		/// <param name="table">Table.</param>
		public static SqlQuery GetTables(Table table)
		{
			string query = "SELECT * FROM"
				+ u 
				+ table.DatabaseName
				+ u
				+ "."
				+ u
				+ table.TableName
				+ u
				+ ";";
			return new SqlQuery (query, false);
		}

		/// <summary>
		/// INSER the specified table.
		/// </summary>
		/// <param name="table">Table.</param>
		public static SqlQuery INSERT(Table table)
		{
			string query = "INSERT INTO "
				+ u 
				+ table.DatabaseName
				+ u
				+ "."
				+ u
				+ table.TableName
				+ u
				+ "(";

			string valueStr = "VALUES(";

			foreach (var property in table.Properties) {
				if (property == table.PRIMARYKEY) {
					if (table.Properties.IndexOf (property) == (table.Properties.Count - 1)) {
						query += ")";
						valueStr += ");";
					}
					continue;
				}
				query 
				+= u
					+ property.PropertyName
					+ u;

				valueStr += getValue (property.Value);

				if (table.Properties.IndexOf (property) == (table.Properties.Count - 1)) {
					query += ")";
					valueStr += ");";
				} else {
					if (table.Properties[table.Properties.IndexOf(property) +1] != table.PRIMARYKEY) {
						query += ",";
						valueStr += ",";
					}
				}
				
			}
			return new SqlQuery((query + valueStr),false); 
		}

		private static string getValue(object value)
		{
			string strValue = value.ToString ();

			Type valueType = value.GetType ();

			Type s = typeof(string);
			Type i = typeof(int);
			Type d = typeof(double);
			Type f = typeof(float);

			if (valueType == s) {
				return (upKomma + strValue + upKomma);
			} else if(valueType == d) {

				var splitValue = strValue.Split(',');

				return (splitValue[0] + "." + splitValue[1]);
			}else
			{
				return strValue;
			}
		}


		/*SELECT * FROM `entityTest`.`Person`;

		*/

		/*
		 * INSERT INTO `entityTest`.`Person`
			(`Name`,
			`Nachname`,
			`Gehalt`)
			VALUES('Jonas','Ahlf',23.0);
*/

	}
}

