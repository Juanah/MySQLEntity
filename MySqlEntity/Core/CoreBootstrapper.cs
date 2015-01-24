using System;
using Microsoft.Practices.Prism.UnityExtensions;
using System.Windows;
using log4net;
namespace Core
{
	public class CoreBootstrapper: UnityBootstrapper 
	{
		ILog Log = LogManager.GetLogger (typeof(CoreBootstrapper));

		#region implemented abstract members of Bootstrapper

		protected override void ConfigureModuleCatalog ()
		{
			base.ConfigureModuleCatalog ();
		}

		protected override void ConfigureContainer ()
		{
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

