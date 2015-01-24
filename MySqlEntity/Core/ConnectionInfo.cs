using System;
using Infrastructure.Core;
using Microsoft.Practices.ServiceLocation;

namespace Core
{
	public class ConnectionInfo:IDBConnectionInfo 
	{

		public ConnectionInfo ()
		{
		}
		#region IDBConnectionInfo implementation
		public string GetSevername ()
		{
			return this.Servername;
		}

		public string GetDatabasename ()
		{
			return this.Databasename;
		}

		public string GetUser ()
		{
			return this.User;
		}

		public string GetPassword ()
		{
			return this.Password;
		}

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
		public string Servername{ get; set; }
		public string Databasename{ get; set; }
		public string User{ get; set; }
		public string Password{ get; set; }
		public string ConnectionString{ get; set; }
	}
}

