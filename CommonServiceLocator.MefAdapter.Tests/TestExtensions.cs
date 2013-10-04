using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Mef.CommonServiceLocator.Tests
{
	public static class TestExtensions
	{
		public static void ShouldBeInstanceOfType(this object instance, Type serviceType, string message)
		{
			var instanceType = instance.GetType();
			Assert.IsTrue(serviceType.IsAssignableFrom(instanceType), message);
		}
	}
}