using System;
using Common;

namespace Infrastructure.Core
{
	public interface IParser
	{



		SqlQuery getSQLQuery(Table table,bool exists);

	}
}

