using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Mef.CommonServiceLocator.Tests
{
	public interface ILogger
	{ }

	[Export(typeof(ILogger))]
	[Export(typeof(SimpleLogger))]
	public class SimpleLogger : ILogger { }

	[Export(typeof(ILogger))]
	[Export(typeof(AdvancedLogger))]
	public class AdvancedLogger : ILogger { }

	[TestClass]
	public abstract class ServiceLocatorTestCase
	{
		private IServiceLocator locator;

		[TestInitialize]
		public void SetUp()
		{
			locator = CreateServiceLocator();
		}

		protected abstract IServiceLocator CreateServiceLocator();

		[TestMethod]
		public void GetInstance()
		{
			var instance = locator.GetInstance<ILogger>();
			Assert.IsNotNull(instance, "instance should not be null");
		}

		[TestMethod]
		[ExpectedException(typeof(ActivationException))]
		public void AskingForInvalidComponentShouldRaiseActivationException()
		{
			locator.GetInstance<IDictionary>();
		}

		[TestMethod]
		public void GetNamedInstance()
		{
			var instance = locator.GetInstance<ILogger>(typeof(AdvancedLogger).FullName);
			instance.ShouldBeInstanceOfType(typeof(AdvancedLogger), "Should be an advanced logger");
		}

		[TestMethod]
		public void GetNamedInstance2()
		{
			var instance = locator.GetInstance<ILogger>(typeof(SimpleLogger).FullName);
			instance.ShouldBeInstanceOfType(typeof(SimpleLogger), "Should be a simple logger");
		}

		[TestMethod]
		[ExpectedException(typeof(ActivationException))]
		public void GetNamedInstance_WithZeroLenName()
		{
			locator.GetInstance<ILogger>("");
		}

		[TestMethod]
		[ExpectedException(typeof(ActivationException))]
		public void GetUnknownInstance2()
		{
			locator.GetInstance<ILogger>("test");
		}

		[TestMethod]
		public void GetAllInstances()
		{
			IEnumerable<ILogger> instances = locator.GetAllInstances<ILogger>();
			IList<ILogger> list = new List<ILogger>(instances);
			Assert.AreEqual(2, list.Count);
		}

		[TestMethod]
		public void GetlAllInstance_ForUnknownType_ReturnEmptyEnumerable()
		{
			IEnumerable<IDictionary> instances = locator.GetAllInstances<IDictionary>();
			IList<IDictionary> list = new List<IDictionary>(instances);
			Assert.AreEqual(0, list.Count);
		}

		[TestMethod]
		public void GenericOverload_GetInstance()
		{
			Assert.AreEqual(
				locator.GetInstance<ILogger>().GetType(),
				locator.GetInstance(typeof(ILogger), null).GetType(),
				"should get the same type"
				);
		}

		[TestMethod]
		public void GenericOverload_GetInstance_WithName()
		{
			Assert.AreEqual(
				locator.GetInstance<ILogger>(typeof(AdvancedLogger).FullName).GetType(),
				locator.GetInstance(typeof(ILogger), typeof(AdvancedLogger).FullName).GetType(),
				"should get the same type"
				);
		}

		[TestMethod]
		public void Overload_GetInstance_NoName_And_NullName()
		{
			Assert.AreEqual(
				(object) locator.GetInstance<ILogger>().GetType(),
				locator.GetInstance<ILogger>(null).GetType(),
				"should get the same type"
				);
		}

		[TestMethod]
		public void GenericOverload_GetAllInstances()
		{
			var genericLoggers = new List<ILogger>(locator.GetAllInstances<ILogger>());
			var plainLoggers = new List<object>(locator.GetAllInstances(typeof(ILogger)));
			Assert.AreEqual(genericLoggers.Count, plainLoggers.Count);
			for (int i = 0; i < genericLoggers.Count; i++)
			{
				Assert.AreEqual(
					genericLoggers[i].GetType(),
					plainLoggers[i].GetType(),
					"instances (" + i + ") should give the same type");
			}
		}
	}
}