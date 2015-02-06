using System;
using Core;
using Infrastructure.Core;
using System.Collections.Generic;
using Infrastructure;

namespace DITest
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Entity test with DI-Container");
			IDBConnectionInfo connectionInfo = new ConnectionInfo () {
				Databasename = "TestDBDI",
				User = "mysqlentity",
				Password = "123",
				Servername = "localhost"
			};
			EntityInitializer initializer = new EntityInitializer (connectionInfo);

			List<IEntity> entities = new List<IEntity> ();
			entities.Add (new Kunde());

			Context context = initializer.GetContext (entities);

			if (context.CreateDatabase ()) {
				Console.WriteLine ("database has been created");
			} else {
				Console.WriteLine ("ERROR database could not be created");
				Console.ReadKey ();
				return;
			}

			context.Parse ();

			if (context.Create ()) {
				Console.WriteLine ("Tables have been created");
			} else {
				Console.WriteLine ("ERROR Tables could not be created");
				Console.ReadKey ();
				return;
			}

			Console.ReadKey ();

		}
	}
}
