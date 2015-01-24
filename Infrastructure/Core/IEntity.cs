using System;

namespace Infrastructure
{
	public interface IEntity
	{
		object DeepCopy ();

		int GetId();
	}
}

