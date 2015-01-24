using System;
using System.Collections.Generic;
using Common;
using Infrastructure.Core;
using Infrastructure;
using log4net;
using System.Linq;

namespace Core
{
	public class Context
	{
		ILog log = log4net.LogManager.GetLogger
			(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType); 

		private static BaseParser mBaseParser = new BaseParser();

		public Context ()
		{
			this.Tables = new List<Table> ();
			this.mDecoder = new BaseDecoder ();
			LoggerConfig.Setup ();
		}
		

		public Context (IDBConnectionInfo info)
		{
			this.ConnectionInfo = info;
			this.Tables = new List<Table> ();
		}

		public Context (IDBConnectionInfo info,List<IEntity> entities)
		{
			this.Entities = entities;
			this.ConnectionInfo = info;
			this.Tables = new List<Table> ();
		}

		public virtual bool CreateDatabase()
		{
			if (Connection == null) {
				log.Info ("Connection is Null, create one");
				Connection = new Connection (this.ConnectionInfo);
			}
			return Connection.ExecuteQuery (new SqlQuery ("CREATE SCHEMA `" + ConnectionInfo.GetDatabasename() + "`", false));
		}

		public virtual bool Create()
		{
			CreateParser createParser = new CreateParser ();

			List<SqlQuery> queries = new List<SqlQuery> ();

			foreach (var table in Tables) {
				queries.Add (createParser.getSQLQuery (table));
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

		public bool Insert<TEntity>(TEntity entity)
		{
			List<Table> table = mBaseParser.getTable (entity, ConnectionInfo.GetDatabasename ());

			foreach (var item in table) {
				SqlQuery query = BaseQueryBuilder.INSERT (item);

				if (!Connection.ExecuteQuery (query)) {
					log.Error ("Inserst failed:" + query);
					return false;
				}
			}

			return true;
		}


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

		public virtual void Parse()
		{
			BaseParser rawPaser = new BaseParser ();

			foreach (var entity in Entities) {
				Tables.AddRange (rawPaser.getTable (entity,ConnectionInfo.GetDatabasename()));
			}
		}


		public List<IEntity> Entities{ get; set; }
		public List<Table> Tables{ get; set; }
		public IDBConnectionInfo ConnectionInfo{ get;private set; }
		public IConnection Connection{ get; private set; }
		public IDecoder mDecoder{ get; set; }
	}
}

