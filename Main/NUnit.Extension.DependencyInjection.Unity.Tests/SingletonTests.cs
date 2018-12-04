using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NUnit.Extension.DependencyInjection.Unity.Tests
{
  [TestFixture]
  public class SingletonTests
  {
    internal class TestData {}

    [Test]
    public void Instance_returns_the_same_instance_after_subsequent_calls()
    {
      var instance1 = Singleton<TestData>.Instance;
      var instance2 = Singleton<TestData>.Instance;
      Assert.That(instance1, Is.SameAs(instance2));
    }
  }
}
