using System;
using Infrastructure;

namespace Core
{
	[Obsolete("Not in use atm")]
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

