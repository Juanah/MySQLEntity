using System;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Infrastructure;

namespace Core
{
	public class CoreModul: IModule
	{
		readonly IUnityContainer mContainer;
		public CoreModul (IUnityContainer container)
		{
			if (container == null)
				throw new ArgumentNullException ("container");
			mContainer = container;
		}

		public void Initialize ()
		{

		}


	}
}

