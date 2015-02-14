using System;
using Core;
using Infrastructure.Core;
using System.Collections.Generic;
using Infrastructure;
using System.Linq;

namespace DITest
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Entity test with DI-Container");
			IDBConnectionInfo connectionInfo = new ConnectionInfo () {
				Databasename = "TestDBDI",
				User = "root",
				Password = "",
				Servername = "localhost"
			};
			EntityInitializer initializer = new EntityInitializer (connectionInfo);

			List<IEntity> entities = new List<IEntity> ();
			entities.Add (new Kunde());
			entities.Add (new Product ());

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


			Console.WriteLine ("Write number of Entities wich should be created");
			int amountOfEntities = Convert.ToInt32(Console.ReadLine ());

			var strRnd = new RandomStringGenerator ();

			for (int i = 1; i <= amountOfEntities; i++) {
				var customer = new Kunde () {
					Name = strRnd.Generate(10,32),
					LastName = strRnd.Generate(5,50)
				};
				context.Insert (customer);
				Console.WriteLine ("Inserted Customer:" + customer.Name + " Index:" + i.ToString ());
			}


			var readCustomer = context.GetTable<Kunde> (typeof(Kunde));

			foreach (var cus in readCustomer) {
				Console.WriteLine ("ReadCustomer: ID:"+ cus.Id + "NAME:"  + cus.Name + " LastName:" + cus.LastName);
			}


			Console.WriteLine ("Foreignkeytest");

			Kunde foreignKunde = new Kunde () {
				LastName = "Entity",
				Name = "Framework"
			};

			context.Insert (foreignKunde);

			var product = new Product () {
				Name = "TestProduct",
				Customer = foreignKunde
			};

			context.Insert (product);

			var readProduct = context.GetTable<Product> (typeof(Product)).FirstOrDefault (); 

			Console.WriteLine ("Readproduct:" + readProduct.Name + " ForeignCustomer:" + readProduct.Customer.Name);

			Console.ReadKey ();

		}
	}
}
