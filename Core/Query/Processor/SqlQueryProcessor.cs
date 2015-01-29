using System;
using Infrastructure;
using Common;
using MySql.Data.MySqlClient;

namespace Core
{
	/// <summary>
	/// Sql query processor.
	/// </summary>
	public class SqlQueryProcessor: ISqlQueryProcessor 
	{
		private IConnection _connection;
		private static string u = "`";
		public SqlQueryProcessor (Infrastructure.IConnection _connection)
		{
			if (_connection == null) {
				throw new ArgumentNullException ("_connection");
			}
			this._connection = _connection;
		}

		#region ISqlQueryProcessor implementation

		public bool Update (Table table)
		{
			if (!Automaticreconnect ()) {
				return false;
			}
			var mysqlCommand = CreateCommand ();
			var sqlQuery = "UPDATE SET `" + table.DatabaseName + "`.`" + table.TableName + "`"; 

			bool isLast = false;
			foreach (var property in table.Properties) {
				if (table.Properties.IndexOf (property) == (table.Properties.Count - 1)) {
					isLast = true;
				}
				if(property == table.PRIMARYKEY && !table.AUTOINCREMENT)
				{
					if(isLast)
					{
						sqlQuery += "NULL";
					}else
					{
						sqlQuery += "NULL, ";
					}
					continue;
				}
				if (isLast) {
					sqlQuery += "@" + property.PropertyName;
				}else
				{
					sqlQuery += "@" + property.PropertyName + ", ";
				}
				mysqlCommand.Parameters.AddWithValue(property.PropertyName,property.Value);
			}

			sqlQuery += ") WHERE " + "`"+ table.PRIMARYKEY.PropertyName + "`=" + ((int)table.PRIMARYKEY.Value).ToString() + ";";
			mysqlCommand.CommandText = sqlQuery;
			if (mysqlCommand.ExecuteNonQuery () < 1) {
				return false;
			}
			return true;
		}

		public bool Insert (Table table)
		{
			if (!Automaticreconnect ()) {
				return false;
			}
			var mysqlCommand = CreateCommand ();
			var sqlQuery = "INSERT INTO `" + table.DatabaseName + "`.`" + table.TableName + "`"; 

			bool isLast = false;
			foreach (var property in table.Properties) {
				if (table.Properties.IndexOf (property) == (table.Properties.Count - 1)) {
					isLast = true;
				}
				if(property == table.PRIMARYKEY && table.AUTOINCREMENT)
				{
					if(isLast)
					{
						sqlQuery += "NULL";
					}else
					{
						sqlQuery += "NULL, ";
					}
					continue;
				}
				if (isLast) {
					sqlQuery += "@" + property.PropertyName;
				}else
				{
					sqlQuery += "@" + property.PropertyName + ", ";
				}
				mysqlCommand.Parameters.AddWithValue(property.PropertyName,property.Value);
			}

			sqlQuery += ");";
			mysqlCommand.CommandText = sqlQuery;
			if (mysqlCommand.ExecuteNonQuery () < 1) {
				return false;
			}
			return true;
		}

		/// Gets the table string.
		/// </summary>
		/// <returns>The table string.</returns>
		/// <param name="table">Table.</param>
		private static string GetTableStr(Table table,bool ifNotExists = false)
		{
			char up = '`';
			string tableQuery = "CREATE TABLE ";
			if (ifNotExists) {
				tableQuery = "CREATE TABLE IF NOT EXISTS ";
			}
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
			} 
			return tableQuery;
		}

		private static string getType(Property property)
		{
			Type p = property.ValueType;
			Type s = typeof(string);
			Type i = typeof(int);
			Type d = typeof(double);
			Type f = typeof(float);
			Type b = typeof(byte[]);
			if (p == s) {
				return "VARCHAR(1000)";
			} else if (p == i) {
				return "INT";
			} else if (p == d) {
				return "DOUBLE";
			} else if (p == f) {
				return "FLOAT";
			} else if (p == b) {
				return "BINARY";
			}
			else {
				return "";
			}
		}

		public bool Create (Table table)
		{
			if (!Automaticreconnect ()) {
				return false;
			}
			var mysqlCommand = CreateCommand ();
			var sqlQuery = GetTableStr (table, true);
			mysqlCommand.CommandText = sqlQuery;
			if(mysqlCommand.ExecuteNonQuery () < 1)
			{
				return false;
			}
			return true;
		}

		public bool Delete (Table table)
		{
			MySqlCommand command = CreateCommand ();
			command.CommandText = DELETE (table);

			if(command.ExecuteNonQuery() < 1)
			{
				return false;
			}else
			{
				return true;
			}
		}

		#endregion

		private bool Automaticreconnect()
		{
			if (!_connection.isOpen ()) {
				try {
					_connection.Close();
					System.Threading.Thread.Sleep(200);
					return _connection.Open();
				} catch (Exception ex) {
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// DELET the specified table.
		/// pattern DELETE FROM `entityTest`.`Person` WHERE `Id`='1';
		/// </summary>
		/// <param name="table">Table.</param>
		public static string DELETE(Table table)
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
			return query;
		}

		private MySqlCommand CreateCommand()
		{
			var command = new MySqlCommand ();
			command.Connection = (MySqlConnection)_connection.GetDbConnection();
			return command;
		}

		private string getSqlStatementSyntax(SQLCommand sqlCommand,string databasename,string tablename)
		{
			switch (sqlCommand) {
				case SQLCommand.Insert:
				return "INSERT INTO `" + databasename + "`.`" + tablename + "`"; 
				case SQLCommand.Update:
				return "UPDATE INTO `" + databasename + "`.`" + tablename + "`"; 
				case SQLCommand.Create:
				return "CREATE TABLE IF NOT EXISTS " + '`' + databasename + '`' + ".`" + tablename + '`';
				default:
				return "";
			}

		}


	}
}

