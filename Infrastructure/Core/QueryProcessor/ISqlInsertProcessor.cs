using System;
using Common;

namespace Infrastructure
{
	/// <summary>
	/// Processor for Insert Queries in form of Table Entities
	/// </summary>
	public interface ISqlInsertProcessor
	{
		bool Insert(Table table);
	}
}

