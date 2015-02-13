using System;

namespace Infrastructure
{
	public interface IEntity
	{
		object DeepCopy ();

		int GetId();

		void SetID(int id);
	}
}

