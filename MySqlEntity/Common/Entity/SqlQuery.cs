using System;

namespace Common
{
	public class SqlQuery
	{
		public SqlQuery ()
		{
		}

		public SqlQuery (string query, bool error)
		{
			this.Query = query;
			this.Error = error;
		}
		

		public string Query{get;set;}

		public bool Error{ get; set;}
	}
}

