using System;
using Infrastructure.Core;
using System.Collections.Generic;
using Infrastructure;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;


namespace Core
{
	/// <summary>
	/// initialize the nessary Classes and so on.
	/// </summary>
	public class EntityInitializer
	{

		IUnityContainer container;

		public EntityInitializer (IDBConnectionInfo _connectionInfo)
		{
			this._connectionInfo = _connectionInfo;
			container = new UnityContainer ();
		}

		public Context GetContext(List<IEntity> entities)
		{
			Init ();
			Context context = new Context (
				this.container.Resolve<IClassParser>(),
				this.container.Resolve<ISqlQueryProcessor>(),
				_connectionInfo
				);
			context.Entities = entities;
			return context;
		}

		private void Init()
		{

			IConnection _connection = new Connection (_connectionInfo);

			this.container.RegisterInstance<IDBConnectionInfo>(_connectionInfo, new ContainerControlledLifetimeManager ());
			this.container.RegisterInstance<IConnection> (_connection, new ContainerControlledLifetimeManager ());
			this.container.RegisterType<ISqlQueryProcessor,SqlQueryProcessor> (new ContainerControlledLifetimeManager ());
			this.container.RegisterType<IClassParser,BaseParser>(new ContainerControlledLifetimeManager ());


			if (_connectionInfo == null) {
				throw new ArgumentNullException ("IDBConnectionInfo");
			}
			//CoreBootstrapper coreBootstrapper = new CoreBootstrapper (_connectionInfo);
			//coreBootstrapper.Run ();
		
		}

		private IDBConnectionInfo _connectionInfo;
	}
}

