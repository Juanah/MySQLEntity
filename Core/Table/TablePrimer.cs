using System;
using System.Collections.Generic;
using Common;
using Infrastructure;
using Infrastructure.Core;

namespace Core
{
	public class TablePrimer: ITablePrimer
	{
		private IClassParser _classParser;


		public TablePrimer (Infrastructure.Core.IClassParser _classParser)
		{
			this._classParser = _classParser;
		}

		[Obsolete("What the ..")]
		public List<Table> PrepareTable(Table table)
		{
			var tables = new List<Table> ();

			foreach (var item in table.Properties) {

			}

			return null;
		}



	}
}

