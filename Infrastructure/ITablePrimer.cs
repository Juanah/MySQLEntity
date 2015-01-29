using System;
using System.Collections.Generic;
using Common;

namespace Infrastructure
{
	public interface ITablePrimer
	{
		List<Table> PrepareTable (Table table);
	}
}

