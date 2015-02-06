using System;
using Infrastructure;
using Infrastructure.Core;
using Common;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Core
{
	/// <summary>
	/// Sql query processor.
	/// </summary>
	public class SqlQueryProcessor: ISqlQueryProcessor 
	{
		private IConnection _connection;
		private IDBConnectionInfo _connectionInfo;


		private static string u = "`";
		public SqlQueryProcessor (IConnection _connection,IDBConnectionInfo connectionInfo)
		{
			if (_connection == null) {
				throw new ArgumentNullException ("IConnection");
			}
			if (connectionInfo == null) {
				throw new ArgumentNullException ("IDBConnectionInfo");
			}
			this._connection = _connection;
			this._connectionInfo = connectionInfo;
		}

		#region ISqlQueryProcessor implementation

		public bool CreateDatabase ()
		{
			var command = CreateCommand ();
			command.CommandText = "CREATE SCHEMA IF NOT EXISTS `" + _connectionInfo.GetDatabasename () + "`";
			if (command.ExecuteNonQuery () < 1) {
				return false;
			}
			return true;
		}

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
			var sqlQuery = "INSERT INTO `" + table.DatabaseName + "`.`" + table.TableName + "` VALUES("; 

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
			mysqlCommand.ExecuteNonQuery ();
			return true;
		}

		public bool Delete (Table table)
		{
			MySqlCommand command = CreateCommand ();
			command.CommandText = DELETE (table);

			command.ExecuteNonQuery (); //Bug from MysqlConnector it does not return affectedRows
			return true;
		}

		public System.Collections.Generic.List<System.Collections.Generic.List<object>> GetTable (Table table)
		{
			int columns = table.Properties.Count;
			string query = "SELECT * FROM`" + table.DatabaseName + "`.`" + table.TableName + "`;";
			MySqlCommand command = CreateCommand ();
			command.CommandText = query;
			MySqlDataReader reader = command.ExecuteReader();

			//0=Row1=columnValues
			List<List<object>> list = new List<List<object>>();

			while (reader.Read()) {
				List<object> tempObjectList = new List<object>();

				for (int i = 0; i < columns; i++) {
					tempObjectList.Add(reader[i]);
				}
				list.Add(tempObjectList);
			}
			return list;
		}
		/*string query = "SELECT * FROM"
				+ u 
				+ table.DatabaseName
				+ u
				+ "."
				+ u
				+ table.TableName
				+ u
				+ ";";

		*/
		#endregion

		private bool Automaticreconnect()
		{
			if (!_connection.isOpen ()) {
				try {
					_connection.Close();
					System.Threading.Thread.Sleep(200);
					return _connection.Open();
				} catch (Exception) {
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
			if (!_connection.isOpen ()) {
				if (!_connection.Open ()) {
					//throw new Exception ("could not open dbConnection");
				}
			}
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

