using System;
using Infrastructure;

namespace DITest
{
	public class Product:BaseEntity
	{
		public Product ()
		{
		}

		public string Name{ get; set;}

		[ForeignKey(typeof(Kunde))]
		public Kunde Customer{get;set;}

	}
}

