using System;

namespace Common
{
	/// <summary>
	/// Represents a sqlQuery wich can be executed by MySql databases
	/// </summary>
	public class SqlQuery
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Common.SqlQuery"/> class.
		/// </summary>
		public SqlQuery ()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Common.SqlQuery"/> class.
		/// </summary>
		/// <param name="query">Query.</param>
		public SqlQuery (string query)
		{
			this.Query = query;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Common.SqlQuery"/> class.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="error">If set to <c>true</c> error.</param>
		[Obsolete("no longer needed")]
		public SqlQuery (string query, bool error)
		{
			this.Query = query;
			this.Error = error;
		}
		
		/// <summary>
		/// raw mySQLQuery 
		/// </summary>
		/// <value>The query.</value>
		public string Query{get;set;}

		[Obsolete("there is no need")]
		public bool Error{ get; set;}
	}
}

