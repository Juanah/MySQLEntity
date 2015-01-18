using System;
using System.Collections.Generic;
using Core;
using Common;
using Infrastructure.Core;
using Infrastructure;
using log4net;

namespace MySqlEntity
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
			this.mConnectionInfo = info;
			this.Tables = new List<Table> ();
		}

		public Context (IDBConnectionInfo info,List<IEntity> entities)
		{
			this.Entities = entities;
			this.mConnectionInfo = info;
			this.Tables = new List<Table> ();
		}

		public virtual bool CreateDatabase()
		{
			if (mConnection == null) {
				log.Info ("Connection is Null, create one");
				mConnection = new Connection (this.mConnectionInfo);
			}
			return mConnection.ExecuteQuery (new SqlQuery ("CREATE SCHEMA `" + mConnectionInfo.GetDatabasename() + "`", false));
		}

		public virtual bool Create()
		{
			CreateParser createParser = new CreateParser ();

			List<SqlQuery> queries = new List<SqlQuery> ();

			foreach (var table in Tables) {
				queries.Add (createParser.getSQLQuery (table));
			}

			mConnection = new Connection (this.mConnectionInfo);
			if (!mConnection.Open()) {
				log.Error ("could not open Database connection");
				return false;
			}

			foreach (var item in queries) {
				log.Info ("Executing Query:" + item.Query);
				if (!mConnection.ExecuteQuery (item)) {
					log.Error ("could not Execute Query:" + item.Query + "Error Level:" + item.Error);
					return false;
				}
			}

			return true;

		}

		public bool Insert(IEntity entity)
		{
			Table table = mBaseParser.getTable (entity, mConnectionInfo.GetDatabasename ());

			SqlQuery query = BaseQueryBuilder.INSERT (table);

			if (!mConnection.ExecuteQuery (query)) {
				log.Error ("Inserst failed:" + query);
				return false;
			}
			return true;
		}


		public List<Object> GetTable(IEntity target)
		{
			if (this.mDecoder == null)
				this.mDecoder = new BaseDecoder ();
			List<Object> objects = new List<object> ();

			Table table = mBaseParser.getTable (target, mConnectionInfo.GetDatabasename ());

			SqlQuery query = BaseQueryBuilder.GetTables (table);

			var result = mConnection.ExecuteReaderQuery (query, table.Properties.Count);

			foreach (var array in result) {
				object clone = ((IEntity)target).DeepCopy ();
				var convertedObject = mDecoder.Decode (array, clone);
				objects.Add (convertedObject);
			}

			return objects;
		}

		public virtual void Parse()
		{
			BaseParser rawPaser = new BaseParser ();



			foreach (var entity in Entities) {
				Tables.Add (rawPaser.getTable (entity,mConnectionInfo.GetDatabasename()));
			}
		}


		public List<IEntity> Entities{ get; set; }
		public List<Table> Tables{ get; set; }
		public IDBConnectionInfo mConnectionInfo;
		public IConnection mConnection;
		public IDecoder mDecoder{ get; set; }
	}
}

