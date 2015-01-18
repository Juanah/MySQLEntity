using System;
using Common;
using System.Collections.Generic;

namespace Infrastructure
{
	public interface IDecoder
	{
		object Decode (List<Object> objects,Object parent);
	}
}

