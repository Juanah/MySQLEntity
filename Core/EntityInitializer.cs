using System;
using Infrastructure.Core;
using System.Collections.Generic;
using Infrastructure;
using Microsoft.Practices.ServiceLocation;


namespace Core
{
	/// <summary>
	/// initialize the nessary Classes and so on.
	/// </summary>
	public class EntityInitializer
	{
		public EntityInitializer (IDBConnectionInfo _connectionInfo)
		{
			this._connectionInfo = _connectionInfo;
		}

		public Context GetContext(List<IEntity> entities)
		{
			Init ();

			Context context = new Context (
				ServiceLocator.Current.GetInstance<IClassParser> (),
				ServiceLocator.Current.GetInstance<ISqlQueryProcessor> (),
				_connectionInfo
				);
			context.Entities = entities;
			return context;
		}

		private void Init()
		{
			if (_connectionInfo == null) {
				throw new ArgumentNullException ("IDBConnectionInfo");
			}
			CoreBootstrapper coreBootstrapper = new CoreBootstrapper (_connectionInfo);
			coreBootstrapper.Run ();
		
		}

		private IDBConnectionInfo _connectionInfo;
	}
}

