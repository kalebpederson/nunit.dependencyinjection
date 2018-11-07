using System.Linq;
using NUnit.Extension.DependencyInjection.Tests;
using NUnit.Framework;

namespace NUnit.Extension.DependencyInjection.Unity.Tests
{
    public class InjectableTypeDiscovererTests
    {
        [Test]
        public void Initialize_creates_registrations_for_matching_mapped_types()
        {
          UnityContainerInitializer.Initialize();
          var container = UnityContainerHolder.Instance;
          Assert.That(
            container.Registrations.Select(x => x.RegisteredType),
            Has.Some.EqualTo(typeof(ITestSettings))
          );
          Assert.That(
            container.Registrations.Any(x =>
              x.RegisteredType == typeof(ITestSettings) && x.MappedToType == typeof(TestSettings)),
            Is.True
          );
        }
    }
}