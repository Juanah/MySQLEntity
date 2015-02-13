using System;
using Infrastructure;

namespace DITest
{
	public class Person:IEntity
	{
		public Person ()
		{
		}

		[IDKey(true)]
		public int Id{ get; set; }

		public string Name{ get; set; }

		public string Nachname{ get; set; }

		public double Gehalt{ get; set; }

		[ForeignKey(typeof(KontaktDaten))]
		public KontaktDaten KontaktDaten{ get; set; }

		public override string ToString ()
		{
			return string.Format ("[Person: Id={0}, Name={1}, Nachname={2}, Gehalt={3}]", Id, Name, Nachname, Gehalt);
		}

		#region IEntity implementation
		public object DeepCopy ()
		{
			return (Person)this.MemberwiseClone ();
		}
		public int GetId ()
		{
			return Id;
		}
		public void SetID (int id)
		{
			Id = id;
		}
		#endregion
	}
}

