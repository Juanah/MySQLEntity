using System;
using System.Collections;
using System.Collections.Generic;
using log4net;
namespace DITest
{
	public static class EnumHelper
	{

		static EnumHelper()
		{
			GlobalCommandsStringKey = new Dictionary<string,EGlobalCommand>();
		}


		public static TEnum StringToEnum<TEnum>(string obj)
		{
			Type t = typeof(TEnum);



			return (TEnum)(object)null;
		}






		private static void CreateDictionaries()
		{
			#region GlobalCommand
			GlobalCommandsStringKey.Add ("createdb", EGlobalCommand.CreateDb);
			GlobalCommandsStringKey.Add ("createtable", EGlobalCommand.CreateTable);
			GlobalCommandsStringKey.Add ("readdb", EGlobalCommand.ReadDb);

			GlobalCommandsEnumKey.Add (EGlobalCommand.CreateDb, "createdb");
			GlobalCommandsEnumKey.Add (EGlobalCommand.CreateTable, "create");
			GlobalCommandsEnumKey.Add (EGlobalCommand.ReadDb, "readdb");
			#endregion
		}

		public static Dictionary<string,EGlobalCommand> GlobalCommandsStringKey{ get; set; }
		public static Dictionary<EGlobalCommand,string> GlobalCommandsEnumKey{ get; set; }

	}
}

