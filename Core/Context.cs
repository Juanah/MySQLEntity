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


		public Context(IClassParser parser,List<IEntity> entities)
		{
			if (parser == null) {
				throw new ArgumentNullException ("parser");
			}
			this.Tables = new List<Table> ();
			this.mDecoder = new BaseDecoder ();
			this.Entities = entities;
			LoggerConfig.Setup ();
		}

		/// <summary>
		/// Creates the database.
		/// </summary>
		/// <returns><c>true</c>, if database was created, <c>false</c> otherwise.</returns>
		public virtual bool CreateDatabase(bool ifNotExists=false)
		{
			if (Connection == null) {
				log.Info ("Connection is Null, create one");
				Connection = new Connection (this.ConnectionInfo);
			}
			if (ifNotExists) {
				return Connection.ExecuteQuery (new SqlQuery ("CREATE SCHEMA IF NOT EXISTS `" + ConnectionInfo.GetDatabasename() + "`", false));
			}
			return Connection.ExecuteQuery (new SqlQuery ("CREATE SCHEMA `" + ConnectionInfo.GetDatabasename() + "`", false));
		}

		/// <summary>
		/// Create this instance.
		/// </summary>
		public virtual bool Create()
		{
			CreateParser createParser = new CreateParser ();

			List<SqlQuery> queries = new List<SqlQuery> ();

			foreach (var table in Tables) {
				queries.Add (createParser.getSQLQuery (table,true));
			}

			Connection = new Connection (this.ConnectionInfo);
			if (!Connection.Open()) {
				log.Error ("could not open Database connection");
				return false;
			}

			foreach (var item in queries) {
				log.Info ("Executing Query:" + item.Query);
				if (!Connection.ExecuteQuery (item)) {
					log.Error ("could not Execute Query:" + item.Query + "Error Level:" + item.Error);
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
			List<Table> table = _baseParser.getTable (entity, ConnectionInfo.GetDatabasename ());

			foreach (var item in table) {
				SqlQuery query = BaseQueryBuilder.INSERT (item);

				if (!Connection.ExecuteQuery (query)) {
					log.Error ("Inserst failed:" + query);
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Update the specified entity.
		/// </summary>
		/// <param name="entity">Entity.</param>
		/// <typeparam name="TEntity">The 1st type parameter.</typeparam>
		public bool Update<TEntity>(TEntity entity)
		{
			List<Table> table = _baseParser.getTable (entity, ConnectionInfo.GetDatabasename ());

			foreach (var item in table) {
				SqlQuery query = BaseQueryBuilder.UPDATE (item);
				if (!Connection.ExecuteQuery (query)) {
					log.Error ("Update failed:" + query);
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

			SqlQuery query = BaseQueryBuilder.DELETE (table);

			return Connection.ExecuteQuery (query);
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
			SqlQuery query = BaseQueryBuilder.GetTables (table);

			var result = Connection.ExecuteReaderQuery (query, table.Properties.Count);

			foreach (var array in result) {
				var convertedObject = mDecoder.Decode (array, table, this); //(array, clone);
				objects.Add ((TEntity)convertedObject);
			}

			return objects;
		}
		/// <summary>
		/// Parse this instance.
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

