using System;
using Infrastructure.Core;
using System.Data;
using MySql.Data.MySqlClient;
using Infrastructure;
using log4net;
using System.Collections.Generic;
using Common;

namespace Core
{
	/// <summary>
	/// Is the Interface between Framework and MySqlDatabase
	/// </summary>
	public class Connection: IConnection
	{

		private ILog log = LogManager.GetLogger (typeof(Connection));
		/// <summary>
		/// Initializes a new instance of the <see cref="Core.Connection"/> class.
		/// </summary>
		/// <param name="info">Info.</param>
		public Connection (IDBConnectionInfo info)
		{

			DbConnection = new MySqlConnection (info.GetConnectionString ());

		}

		#region IConnection implementation

		public bool isOpen ()
		{
			ConnectionState state = this.DbConnection.State;
			if (state == ConnectionState.Open) {
				return true;
			}
			return false;
		}

		public bool ExecuteQuery (SqlQuery query)
		{
			throw new NotImplementedException ();
		}
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

		public object GetDbConnection ()
		{
			return this.DbConnection;
		}


		#endregion


		/// <summary>
		/// Gets or sets the db connection.
		/// </summary>
		/// <value>The db connection.</value>
		public IDbConnection DbConnection{ get; set; }
	}
}

