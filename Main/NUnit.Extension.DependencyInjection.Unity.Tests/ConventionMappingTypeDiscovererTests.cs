// Copyright (c) Kaleb Pederson Software LLC. All rights reserved.
// Licensed under the MIT license. See LICENSE file alongside the solution file for full license information.

using System.Linq;
using NUnit.Extension.DependencyInjection.Unity.Tests.SampleTypes;
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
      using (var container = new UnityContainer())
      {
        var discoverer = new ConventionMappingTypeDiscoverer();
        discoverer.Discover(container);
        Assert.That(
          container.Registrations.Any(r => r.RegisteredType == typeof(ITestSettings) && r.MappedToType == typeof(TestSettings)),
          Is.True,
          $"No registration of type {typeof(ITestSettings).FullName} that maps to {typeof(TestSettings).FullName}");
      }
    }
    
    [Test]
    public void Discover_excludes_types_decorated_with_NUnitExcludeFromAutoScanAttribute_from_ioc_container_registration()
    {
      using (var container = new UnityContainer())
      {
        var discoverer = new ConventionMappingTypeDiscoverer();
        discoverer.Discover(container);
        Assert.That(
          container.Registrations.Any(r => r.RegisteredType == typeof(IFoo) && r.MappedToType == typeof(Foo)),
          Is.False,
          $"Found a registration which mapped type {typeof(IFoo).FullName} to {typeof(Foo).FullName}");
      }
    }

    [Test]
    public void Discover_excludes_interfaces_that_do_not_have_corresponding_concrete_implementations()
    {
      using (var container = new UnityContainer())
      {
        var discoverer = new ConventionMappingTypeDiscoverer();
        discoverer.Discover(container);
        Assert.That(
          container.Registrations.Any(r => r.RegisteredType.IsAbstract && r.MappedToType is null),
          Is.False,
          "Found a registration for a type without a concrete implementation");
      }
    }
  }
}
