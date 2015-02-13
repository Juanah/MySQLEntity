using System;
using System.Collections.Generic;
using Common;
using Infrastructure.Core;
using Infrastructure;
using log4net;
using System.Linq;

namespace Core
{
	/// <summary>
	/// The Context is the Frameworkcontroller
	/// </summary>
	public class Context
	{
		ILog log = log4net.LogManager.GetLogger
			(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType); 
		private static IClassParser _baseParser;
		private static ISqlQueryProcessor _sqlprocessor;

		public Context(IClassParser parser,ISqlQueryProcessor processor,IDBConnectionInfo connectionInfo)
		{
			if (parser == null) {
				throw new ArgumentNullException ("parser");
			}
			if (processor == null) {
				throw new ArgumentNullException ("processor");
			}
			_baseParser = parser;
			_sqlprocessor = processor;
			this.Tables = new List<Table> ();
			this.mDecoder = new BaseDecoder ();
			LoggerConfig.Setup ();
			this.ConnectionInfo = connectionInfo;
		}

		/// <summary>
		/// Creates the database.
		/// </summary>
		/// <returns><c>true</c>, if database was created, <c>false</c> otherwise.</returns>
		public virtual bool CreateDatabase(bool ifNotExists=false)
		{
			return _sqlprocessor.CreateDatabase ();
		}

		/// <summary>
		/// Creates the Tables
		/// </summary>
		public virtual bool Create()
		{

			foreach (var table in Tables) {
				if (!_sqlprocessor.Create (table)) {
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Insert the specified entity.
		/// </summary>
		/// <param name="entity">Entity.</param>
		/// <typeparam name="TEntity">The 1st type parameter.</typeparam>
		public bool Insert<TEntity>(TEntity entity)
		{

			List<Table> tables = _baseParser.getTable (entity, ConnectionInfo.GetDatabasename ());

			//The Last table is always the Table of the Entity itself

			int indexOfEntity = _sqlprocessor.Insert (tables.Last());

			tables.Remove (tables.Last ());

			foreach (var table in tables) {
				if (table == tables.Last()) {
					continue;
				}
				if (_sqlprocessor.Insert (table) == -1) {
					return false;
				}
			}

			((IEntity)entity).SetID (indexOfEntity);
			return true;
		}

		/// <summary>
		/// Update the specified entity.
		/// </summary>
		/// <param name="entity">Entity.</param>
		/// <typeparam name="TEntity">The 1st type parameter.</typeparam>
		public bool Update<TEntity>(TEntity entity)
		{
			List<Table> tables = _baseParser.getTable (entity, ConnectionInfo.GetDatabasename ());

			foreach (var table in tables) {
				if (!_sqlprocessor.Update (table)) {
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Delete the specified entity.
		/// </summary>
		/// <param name="entity">Entity.</param>
		/// <typeparam name="TEntity">The 1st type parameter.</typeparam>
		public bool Delete<TEntity>(TEntity entity)
		{
			Table table = _baseParser.getTable (entity, ConnectionInfo.GetDatabasename ()).FirstOrDefault(t => t.State == ETableState.Normal);

			return _sqlprocessor.Delete (table);
		}

		/// <summary>
		/// Gets the table.
		/// </summary>
		/// <returns>The table.</returns>
		/// <param name="type">Type.</param>
		/// <typeparam name="TEntity">The 1st type parameter.</typeparam>
		public List<TEntity> GetTable<TEntity>(Type type)
		{
			if (this.mDecoder == null)
				this.mDecoder = new BaseDecoder ();
			List<TEntity> objects = new List<TEntity> ();

			Table table = this.Tables.FirstOrDefault(t => t.OriginalObject.GetType().Equals(type));

			if (table == null) {
				log.Error("table null referenz");
				throw new ArgumentNullException("table","is Null");
			}
			var result = _sqlprocessor.GetTable (table);//Connection.ExecuteReaderQuery (query, table.Properties.Count);

			foreach (var array in result) {
				var convertedObject = mDecoder.Decode (array, table, this); //(array, clone);
				objects.Add ((TEntity)convertedObject);
			}
			return objects;
		}
		/// <summary>
		/// Parses the Tables.
		/// </summary>
		public virtual void Parse()
		{
			foreach (var entity in Entities) {
				Tables.AddRange (_baseParser.getTable (entity,ConnectionInfo.GetDatabasename()));
			}
		}


		public List<IEntity> Entities{ get; set; }
		public List<Table> Tables{ get; set; }
		public IDBConnectionInfo ConnectionInfo{ get;private set; }
		public IConnection Connection{ get; private set; }
		public IDecoder mDecoder{ get; set; }
	}
}

