using System;
using Infrastructure.Core;
using System.Data;
using MySql.Data.MySqlClient;
using Infrastructure;
using log4net;
using System.Collections.Generic;

namespace Core
{
	public class Connection: IConnection
	{

		ILog log = log4net.LogManager.GetLogger
			(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType); 

		public Connection (IDBConnectionInfo info)
		{

			DbConnection = new MySqlConnection (info.GetConnectionString ());

		}

		#region IConnection implementation

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


		/*
		 * using System;
using System.Data;

using MySql.Data;
using MySql.Data.MySqlClient;

public class Tutorial2
{
    public static void Main()
    {
        string connStr = "server=localhost;user=root;database=world;port=3306;password=******;";
        MySqlConnection conn = new MySqlConnection(connStr);
        try
        {
            Console.WriteLine("Connecting to MySQL...");
            conn.Open();

            string sql = "SELECT Name, HeadOfState FROM Country WHERE Continent='Oceania'";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Console.WriteLine(rdr[0]+" -- "+rdr[1]);
            }
            rdr.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

        conn.Close();
        Console.WriteLine("Done.");
    }
}

*/

		public IDbConnection DbConnection{ get; set; }
	}
}

