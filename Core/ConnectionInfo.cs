using System;
using Infrastructure.Core;
using Microsoft.Practices.ServiceLocation;

namespace Core
{
	/// <summary>
	/// Stores the DatabaseInformations
	/// </summary>
	public class ConnectionInfo:IDBConnectionInfo 
	{
		public ConnectionInfo ()
		{
		}
		#region IDBConnectionInfo implementation
		/// <summary>
		/// Gets the severname.
		/// </summary>
		/// <returns>The severname.</returns>
		public string GetSevername ()
		{
			return this.Servername;
		}
		/// <summary>
		/// Gets the databasename.
		/// </summary>
		/// <returns>The databasename.</returns>
		public string GetDatabasename ()
		{
			return this.Databasename;
		}
		/// <summary>
		/// Gets the user.
		/// </summary>
		/// <returns>The user.</returns>
		public string GetUser ()
		{
			return this.User;
		}
		/// <summary>
		/// Gets the password.
		/// </summary>
		/// <returns>The password.</returns>
		public string GetPassword ()
		{
			return this.Password;
		}
		/// <summary>
		/// Gets the connection string.
		/// </summary>
		/// <returns>The connection string.</returns>
		public string GetConnectionString ()
		{
			if (this.ConnectionString == null) {
				this.ConnectionString = 
					"Server =" + this.Servername + ";" +
					//"Database=" + this.Databasename + ";" +
					"User ID=" + this.User + ";" +
					"Password=" + this.Password + ";" +
					"Pooling=false";
			} 
			return this.ConnectionString;
		}

		#endregion
		/// <summary>
		/// Gets or sets the servername.
		/// </summary>
		/// <value>The servername.</value>
		public string Servername{ get; set; }
		/// <summary>
		/// Gets or sets the databasename.
		/// </summary>
		/// <value>The databasename.</value>
		public string Databasename{ get; set; }
		/// <summary>
		/// Gets or sets the user.
		/// </summary>
		/// <value>The user.</value>
		public string User{ get; set; }
		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		/// <value>The password.</value>
		public string Password{ get; set; }
		/// <summary>
		/// Gets or sets the connection string.
		/// </summary>
		/// <value>The connection string.</value>
		public string ConnectionString{ get; set; }
	}
}

