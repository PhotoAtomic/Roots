using System.ComponentModel.Composition.Hosting;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Mef.CommonServiceLocator.Tests
{
	[TestClass]
	public class MefAdapterTestCase : ServiceLocatorTestCase
	{
		protected override IServiceLocator CreateServiceLocator()
		{
			var catalog = new TypeCatalog(typeof(SimpleLogger), typeof(AdvancedLogger));
			var container = new CompositionContainer(catalog);
			var locator = new MefServiceLocator(container);
			return locator;
		}
	}
}