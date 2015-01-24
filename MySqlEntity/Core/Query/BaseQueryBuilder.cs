using System;
using Common;
using Infrastructure;


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

		//Update
		//UPDATE `entityTest`.`Person` SET `Name`='Test3' WHERE `Id`='1';
		public static SqlQuery UPDATE(Table table)
		{
			string query = "UPDATE "
				+ u 
					+ table.DatabaseName
					+ u
					+ "."
					+ u
					+ table.TableName
					+ u
					+ " SET ";


			foreach (var property in table.Properties) {
				if (table.PRIMARYKEY == property) {
					continue;
				}
				query 
					+= u
					+ property.PropertyName
					+ u
					+ "="
					+ getValue (property.Value,property.AttributeTyp,true);



				if (table.Properties.IndexOf (property) == (table.Properties.Count - 1)) {
					query +=  " WHERE " + u+ table.PRIMARYKEY.PropertyName + u + "=" + "'" + ((int)table.PRIMARYKEY.Value).ToString() + "'" +";";
				} else {
					if (table.Properties[table.Properties.IndexOf(property) +1] != table.PRIMARYKEY) {
						query += ",";
					}
				}

			}
			return new SqlQuery((query),false); 

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

				valueStr += getValue (property.Value,property.AttributeTyp);

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


		/// <summary>
		/// DELET the specified table.
		/// pattern DELETE FROM `entityTest`.`Person` WHERE `Id`='1';
		/// </summary>
		/// <param name="table">Table.</param>
		public static SqlQuery DELETE(Table table)
		{
			string query = "DELETE FROM " 
				+ u
				+ table.DatabaseName
				+ u
				+ "."
				+ u
				+ table.TableName
				+ u
				+ " WHERE "
				+ u
				+ table.PRIMARYKEY.PropertyName
				+ u
				+"="
				+ "'"
				+ ((int)table.PRIMARYKEY.Value).ToString()
				+ "';";
		return new SqlQuery (query, false);
		}

		private static string getValue(object value,AttributeTyp atrType,bool update=false)
		{

			if (atrType == AttributeTyp.Foreignkey) {
				if (value.GetType() == typeof(int)) {
					return "'"+ value.ToString () + "'";
				}
				IEntity entity = (IEntity)value;
				return "'" + entity.GetId ().ToString() + "'";
			}

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

				if (update) {
					return ("'" + splitValue[0] + "." + splitValue[1] +"'");
				}

				return (splitValue[0] + "." + splitValue[1]);
			}else
			{
				if (update) {
					return "'" + strValue + "'";

				}
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

