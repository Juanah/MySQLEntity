using System;
using Common;

namespace Infrastructure
{
	/// <summary>
	/// Processor for, update, insert, create and delete sql commands
	/// </summary>
	public interface ISqlQueryProcessor
	{
		bool Update(Table table);

		bool Insert(Table table);

		bool Create(Table table);

		bool Delete(Table table);
	}
}

