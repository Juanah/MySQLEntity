using System;
using MySqlEntity;
using Core;
using System.Collections.Generic;
using Infrastructure;
using System.Linq;
using System.Linq;


namespace MySqlEntityTest
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			TestUmgebung testUmGebung = new TestUmgebung ();

			testUmGebung.doit ();

			Console.WriteLine ("Test DB erstellung");

			var objects = new List<IEntity> ();
			objects.Add (new TestEntity());
			objects.Add (new TestEntity2());
			objects.Add (new Person ());

			Context context = new Context(new ConnectionInfo()
			                              {
				Databasename = "entityTest",
				User = "appserver",
				Password = "040123",
				Servername = "localhost"
			},objects);





			context.Parse ();

			context.CreateDatabase ();

			context.Create ();


			Person person = new Person () {
				Name = "TestPerson",
				Nachname = "TestNachName",
				Gehalt = 2300.34

			};

			Person person2 = new Person () {
				Name = "Jonas",
				Nachname = "Ahlf",
				Gehalt = 300.30
			};

			context.Insert<Person> (person2);

			if (context.Insert<Person>(person)) {
				Console.WriteLine ("Insert successful");
			} else {
				Console.WriteLine ("Insert failed");
			}


			var result = context.GetTable<Person> (typeof(Person)).Where (n => n.Name == "Jonas").ToList ();




			Console.WriteLine (result.Count);


		}
	}
}
