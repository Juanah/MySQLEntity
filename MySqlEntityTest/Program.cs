using System;
using MySqlEntity;
using Core;
using System.Collections.Generic;
using Infrastructure;
using System.Linq;
using log4net;

namespace MySqlEntityTest
{
	class MainClass
	{
		private static Context context;
		private static RandomStringGenerator stringGenerator = new RandomStringGenerator ();
		private static ILog Log = LogManager.GetLogger("MySqlEntityTest");

		public static void Main (string[] args)
		{
			TestUmgebung testUmGebung = new TestUmgebung ();
			testUmGebung.doit ();
			LoggerConfig.Setup ();
			Menu ();
		}

		private static void Menu()
		{
			Command c = new Command ("", null, null);
		
			while (c.MainCommand != "exit") {

				c = GetCommand ();

				if (c == null) {
					Log.Warn ("command: null");
					continue;
				}
				switch (c.MainCommand) {
				case "init":
					InitializeDb ();
					break;
				case "createdb":
					if (!CreateDb ()) {
						Log.Error ("could not create DB");
					}
					ParseTables ();
					InitializeTables ();
					break;
				case "createtable":
					ParseTables ();
					InitializeTables ();
					break;
				case "readdb":
					ReadPersonen ();
					break;
				case "createperson":
					if (!CreateTestPerson (Read ())) {
						Log.Error ("could not create new Person");
					}
					break;
				case "testprimitivlist":
					TestPrimitivStringList ();
					break;
				case "foreigntest":
					TestForeign ();
					break;
				case "updatetest":
					UpdateEntity ();
					break;
				case "delete":
					DeletePerson (Console.ReadLine ());
					break;
				default:
					Log.Warn ("command:" + c.MainCommand + " not found");
						break;
				}
			}
		}

		#region UI Methods
		private static Command GetCommand()
		{
			string input = Read ();

			if (!String.IsNullOrEmpty (input)) {
				return new Command (input, null, null);
			} 

			return new Command ("null", null, null);
		}

		private static string Read()
		{
			return Console.ReadLine ();
		}
		#endregion

		public static void InitializeDb()
		{
			Log.Info ("Test DB erstellung");
			var objects = new List<IEntity> ();
			objects.Add (new TestEntity());
			objects.Add (new TestEntity2());
			objects.Add (new Person ());
			objects.Add (new ForeignKeyTest ());
			objects.Add (new Telefonnummer ());
			objects.Add (new KontaktDaten ());
			context = new Context(new ConnectionInfo()
			                              {
				Databasename = "entityTest",
				User = "appserver",
				Password = "040123",
				Servername = "localhost"
			},objects);
		}

		private static bool CreateDb()
		{
			Log.Info ("Created Database" + context.ConnectionInfo.GetDatabasename());
			return context.CreateDatabase (true);
		}


		private static bool InitializeTables()
		{
			Log.Info ("Create Tables on Database:" + context.ConnectionInfo.GetDatabasename ());
			foreach (var table in context.Tables) {
				Log.Info ("Table :" + table.TableName);
			}
			return context.Create ();
		}

		private static void ParseTables()
		{
			context.Parse ();
		}

		private static void DeletePerson(string name)
		{
			var person = context.GetTable<Person> (typeof(Person)).FirstOrDefault(n => n.Name == "Hannes");

			if (!context.Delete<Person> (person)) {
				Log.Error ("could not delete Entity");
			} else {
				Log.Debug ("Delete Successfull");
			}
		}

		private static void UpdateEntity()
		{
			var person = context.GetTable<Person> (typeof(Person)).FirstOrDefault();

			person.Name = "Hannes";
			if (!context.Update<Person>(person)) {
				Log.Error ("Could not Update Entity");
				return;
			}

			var personRead = context.GetTable<Person> (typeof(Person)).FirstOrDefault(n => n.Name == "Hannes");

			if (personRead != null) {
				Log.Info ("Update Successful");
			} else {
				Log.Warn ("Update Failed");
			}

		}

		private static void TestForeign()
		{

			Telefonnummer telefon = new Telefonnummer ();
			telefon.Tel = "04154/70341";

			if (!context.Insert<Telefonnummer> (telefon)) {
				Log.Error ("could not insert telefonnumber");
				return;
			}

			telefon = context.GetTable<Telefonnummer> (typeof(Telefonnummer)).FirstOrDefault(t => t.Tel == "04154/70341");

			KontaktDaten kontaktDaten = new KontaktDaten ();
			kontaktDaten.Telefonnummer = telefon;


			if (!context.Insert<KontaktDaten> (kontaktDaten)) {
				Log.Error ("could not insert KontaktDaten");
				return;
			}

			kontaktDaten = context.GetTable<KontaktDaten> (typeof(KontaktDaten)).FirstOrDefault(t => t.Telefonnummer.Tel == "04154/70341");

			Person perso = new Person ();
			perso.Nachname = "foreign";
			perso.Name = "Test";
			perso.Gehalt = 200.234;
			perso.KontaktDaten = kontaktDaten;

			if (!context.Insert<Person> (perso)) {
				Log.Error ("Could not insert person");
				return;
			}

			var readPerso = context.GetTable<Person> (typeof(Person)).FirstOrDefault (gnn => (gnn.Gehalt == 200.234) && (gnn.Nachname == "foreign") && (gnn.Name == "Test")); 

			if (readPerso == null) {
				Log.Error ("Read Perso Object is Null");
				return;
			}

			ForeignKeyTest testItem = new ForeignKeyTest ();
			testItem.MeineTestPerson = readPerso;

			if (!context.Insert<ForeignKeyTest> (testItem)) {
				Log.Error ("Insert Foreignkeytest failed!");
				return;
			}


			var readForeignKeyTest = context.GetTable<ForeignKeyTest> (typeof(ForeignKeyTest));

			if (readForeignKeyTest == null) {
				Log.Error ("ReadList of ForeignKeyTest objects == null");
				return;
			}

			foreach (var item in readForeignKeyTest) {
				Log.Info (item.MeineTestPerson.Nachname);
			}
		}

		private static void TestPrimitivStringList()
		{
			List<string> strList = new List<string> ();
			string hallo = "hallo";
			string welt = "welt";
			strList.Add (hallo);
			strList.Add (welt);
			BaseParser parser = new BaseParser ();
			var re = parser.GenerateTableFromList<string> (strList, "strList", "entityTest");
			CreateParser cParser = new CreateParser ();
			var query = cParser.getSQLQuery (re,false);
			Log.Info ("PrimitivList query: " + query.Query); 
		}


		private static bool CreateTestPerson(string name)
		{
			Person person = new Person ();
			person.Name = name;
			person.Nachname = stringGenerator.Generate (8, 32);
			person.Gehalt = GetRandomNumber (450.65, 923000.90);
			Log.Info ("Insert Entity Person :" + person.ToString ());
			return context.Insert<Person> (person);
		}

		private static void ReadPersonen()
		{
			List<Person> personen = context.GetTable<Person> (typeof(Person));
			foreach (var person in personen) {
				Log.Info ("Person :" + person.ToString ());
			}
		}

		public static double GetRandomNumber(double minimum, double maximum)
		{ 
			Random random = new Random();
			return random.NextDouble() * (maximum - minimum) + minimum;
		}
	}
}
