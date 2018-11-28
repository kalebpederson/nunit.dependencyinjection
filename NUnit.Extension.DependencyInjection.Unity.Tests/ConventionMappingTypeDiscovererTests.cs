using System.Linq;
using NUnit.Framework;
using Unity;

namespace NUnit.Extension.DependencyInjection.Unity.Tests
{
  [TestFixture]
  public class ConventionMappingTypeDiscovererTests
  {
    [Test]
    public void Discover_registers_expected_abstract_type_with_provided_ioc_container()
    {
      var container = new UnityContainer();
      var discoverer = new ConventionMappingTypeDiscoverer();
      discoverer.Discover(container);
      Assert.That(
        container.Registrations.Any(r => r.RegisteredType == typeof(ITestSettings) && r.MappedToType == typeof(TestSettings)),
        Is.True,
        $"No registration of type {typeof(ITestSettings).FullName} that maps to {typeof(TestSettings).FullName}"
      );
    }
  }
}
