using System;

namespace Common
{
	/// <summary>
	/// Defines the State of the table 
	/// Normal means the baseTable
	/// </summary>
	public enum ETableState
	{
		Normal,
		Foreign,
		PrimitivList,
		Mapping
	}
}

