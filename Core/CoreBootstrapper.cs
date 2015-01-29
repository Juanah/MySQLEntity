using System;
using Microsoft.Practices.Prism.UnityExtensions;
using System.Windows;
using log4net;
using Microsoft.Practices.Prism.Modularity;
using Infrastructure;
using Microsoft.Practices.Unity;
using Infrastructure.Core;
using System.Collections.Generic;


namespace Core
{
	public class CoreBootstrapper: UnityBootstrapper 
	{

		private IDBConnectionInfo _info;
		private IConnection _connection;
		public CoreBootstrapper (IDBConnectionInfo _info)
		{
			this._info = _info;
		}
		

		#region implemented abstract members of Bootstrapper

		protected override void ConfigureModuleCatalog ()
		{
			base.ConfigureModuleCatalog ();
		}

		protected override void ConfigureContainer ()
		{
			/*
			 * mContainer.RegisterType<IConnection,Connection> (new ContainerControlledLifetimeManager ());
			mContainer.RegisterType<ISqlQueryProcessor,SqlQueryProcessor> (new ContainerControlledLifetimeManager ());
			 * 
			 * */
			_connection = new Connection (_info);
			this.Container.RegisterInstance<IDBConnectionInfo>(_info, new ContainerControlledLifetimeManager ());
			this.Container.RegisterInstance<IConnection> (_connection, new ContainerControlledLifetimeManager ());
			this.Container.RegisterType<ISqlQueryProcessor,SqlQueryProcessor> (new ContainerControlledLifetimeManager ());
			this.Container.RegisterType<IClassParser,BaseParser>(new ContainerControlledLifetimeManager ());
			base.ConfigureContainer ();
		}

		protected override void InitializeShell ()
		{
		}

		protected override DependencyObject CreateShell ()
		{
			return null;
		}

		#endregion
	}
}

