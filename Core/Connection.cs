using System;
using Infrastructure.Core;
using System.Data;
using MySql.Data.MySqlClient;
using Infrastructure;
using log4net;
using System.Collections.Generic;

namespace Core
{
	/// <summary>
	/// Is the Interface between Framework and MySqlDatabase
	/// </summary>
	public class Connection: IConnection
	{

		ILog log = log4net.LogManager.GetLogger
			(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType); 

		/// <summary>
		/// Initializes a new instance of the <see cref="Core.Connection"/> class.
		/// </summary>
		/// <param name="info">Info.</param>
		public Connection (IDBConnectionInfo info)
		{

			DbConnection = new MySqlConnection (info.GetConnectionString ());

		}

		#region IConnection implementation
		/// <summary>
		/// Open the Databaseconnection.
		/// </summary>
		public bool Open ()
		{
			try {
				DbConnection.Open();
				return true;
			} catch (Exception ex) {
				log.Error (ex);
				return false;
			}
		}

		/// <summary>
		/// Closes the Databaseconnection
		/// </summary>
		public bool Close ()
		{
			try {
				DbConnection.Close();
				return true;
			} catch (Exception ex) {
				log.Error (ex);
				return false;
			}
		}
		/// <summary>
		/// Executes the query.
		/// </summary>
		/// <returns><c>true</c>, if query was executed, <c>false</c> otherwise.</returns>
		/// <param name="query">Query.</param>
		public bool ExecuteQuery (Common.SqlQuery query)
		{
			try {
				if (this.DbConnection.State != ConnectionState.Open) {
					if(!Open())
					{
						throw new NotSupportedException();
					}
				}
				using (MySqlCommand command = new MySqlCommand (query.Query, (MySqlConnection)DbConnection)) {
					command.ExecuteNonQuery ();
				}

				Close();

				return true;

			} catch (Exception ex) {
				log.Error ("could not Execute Query!" + ex);
				return false;
			}
		}

		/// <summary>
		/// Executes the reader query.
		/// </summary>
		/// <returns>The reader query.</returns>
		/// <param name="query">Query.</param>
		/// <param name="columns">Columns.</param>
		public List<List<Object>> ExecuteReaderQuery(Common.SqlQuery query,int columns)
		{
			try {
				if (this.DbConnection.State != ConnectionState.Open) {
					if(!Open())
					{
						throw new NotSupportedException();
					}
				}
				MySqlCommand command = new MySqlCommand (query.Query, (MySqlConnection)DbConnection);
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
				Close();
				return list;

			} catch (Exception ex) {
				log.Error ("could not Execute Query!" + ex);
				return null;
			}
		}



		#endregion
		/// <summary>
		/// Gets or sets the db connection.
		/// </summary>
		/// <value>The db connection.</value>
		public IDbConnection DbConnection{ get; set; }
	}
}

