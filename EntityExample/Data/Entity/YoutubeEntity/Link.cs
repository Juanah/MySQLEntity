using System;
using Infrastructure;
namespace Data
{
	public class Link: IEntity
	{
		public Link ()
		{
		}

		[IDKey(true)]
		public int Id{get;set;}

		public string RawLink{ get; set; }





		#region IEntity implementation
		public object DeepCopy ()
		{
			return this.MemberwiseClone ();
		}
		public int GetId ()
		{
			return this.Id;
		}
		#endregion
	}
}

