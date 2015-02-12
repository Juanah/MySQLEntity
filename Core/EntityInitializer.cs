using System;
using Infrastructure.Core;
using System.Collections.Generic;
using Infrastructure;


namespace Core
{
	/// <summary>
	/// initialize the nessary Classes and so on.
	/// </summary>
	public class EntityInitializer
	{
		private ISqlQueryProcessor SqlQueryProcessor;
		private IClassParser ClassParser;
		private IConnection _connection;
		public EntityInitializer (IDBConnectionInfo _connectionInfo)
		{
			this._connectionInfo = _connectionInfo;
		}

		public Context GetContext(List<IEntity> entities)
		{
			Init ();
			Context context = new Context (
				ClassParser,
				SqlQueryProcessor,
				_connectionInfo
				);
			context.Entities = entities;
			return context;
		}

		private void Init()
		{
			_connection = new Connection (_connectionInfo);

			SqlQueryProcessor = new Core.SqlQueryProcessor (_connection, _connectionInfo);
			ClassParser = new BaseParser (_connectionInfo);


			if (_connectionInfo == null) {
				throw new ArgumentNullException ("IDBConnectionInfo");
			}
			//CoreBootstrapper coreBootstrapper = new CoreBootstrapper (_connectionInfo);
			//coreBootstrapper.Run ();
		
		}

		private IDBConnectionInfo _connectionInfo;
	}
}

