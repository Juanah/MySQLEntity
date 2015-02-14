using System;
using Common;
using System.Collections.Generic;

namespace Infrastructure
{
	public interface IDecoder
	{
		object Decode (List<Object> objects,Table table,object context);

		object GetList(string idList,object context);
	}
}

