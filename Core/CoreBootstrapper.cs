using System;
using Microsoft.Practices.Prism.UnityExtensions;
using System.Windows;
using log4net;
using Microsoft.Practices.Prism.Modularity;
using Infrastructure;
using Microsoft.Practices.Unity;
using Infrastructure.Core;


namespace Core
{
	[Obsolete()]
	public class CoreBootstrapper: UnityBootstrapper 
	{
		ILog Log = LogManager.GetLogger (typeof(CoreBootstrapper));

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
			ModuleCatalog moduleCatalog = (ModuleCatalog)this.ModuleCatalog;
			moduleCatalog.AddModule (typeof(CoreModul));
		}

		protected override void ConfigureContainer ()
		{
			/*
			 * mContainer.RegisterType<IConnection,Connection> (new ContainerControlledLifetimeManager ());
			mContainer.RegisterType<ISqlQueryProcessor,SqlQueryProcessor> (new ContainerControlledLifetimeManager ());
			 * 
			 * */
			_connection = new Connection (_info);
			this.Container.RegisterInstance<IConnection> (_connection, new ContainerControlledLifetimeManager ());
			this.Container.RegisterType<ISqlQueryProcessor,SqlQueryProcessor> (new ContainerControlledLifetimeManager ());
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

