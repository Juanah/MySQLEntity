using System;
using Common;
using System.Collections.Generic;

namespace Infrastructure
{
	public interface IConnection
	{
		bool Open();

		bool Close();

		bool isOpen();

		bool ExecuteQuery(SqlQuery query);

		object GetDbConnection (); 
		
		//List<List<Object>> ExecuteReaderQuery (Common.SqlQuery query, int columns);

	}
}

