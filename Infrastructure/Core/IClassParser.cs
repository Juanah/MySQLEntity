using System;
using Common;
using System.Collections.Generic;

namespace Infrastructure.Core
{
	public interface IClassParser
	{
		List<Table> getTable(Object obj,string databasename);
	}
}

