using System;
using Common;

namespace Infrastructure.Core
{
	public interface IClassParser
	{

		Table getTable(Object obj,string databasename);

	}
}

