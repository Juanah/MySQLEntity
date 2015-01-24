using System;
using Infrastructure;

namespace Core
{
	public class PrimitivListDecoder: IDecoder 
	{
		public PrimitivListDecoder ()
		{
		}

		#region IDecoder implementation

		public object Decode (System.Collections.Generic.List<object> objects, Common.Table table, object context)
		{
			throw new NotImplementedException ();
		}
		#endregion
	}
}

