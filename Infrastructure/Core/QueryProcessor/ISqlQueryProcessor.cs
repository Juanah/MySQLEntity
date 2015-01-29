using System;
using Common;
using System.Collections.Generic;

namespace Infrastructure
{
	/// <summary>
	/// Processor for, update, insert, create and delete sql commands
	/// </summary>
	public interface ISqlQueryProcessor
	{
		bool CreateDatabase ();

		bool Update(Table table);

		bool Insert(Table table);

		bool Create(Table table);

		bool Delete(Table table);

		List<List<Object>> GetTable(Table table);
	}
}

