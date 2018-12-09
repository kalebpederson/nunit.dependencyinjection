using System.Linq;
using NUnit.Extension.DependencyInjection;
using NUnit.Framework;
using Unity;

namespace ConventionMappingTypeDiscovererTests
{
  public interface IWillNotHaveARegistration {}
  public class ClassesWithoutAnInterfaceAreNotRegistered {}

  [DependencyInjectingTestFixture]
  public class RegistrationTests
  {
    private readonly IUnityContainer _container;

    public RegistrationTests(IUnityContainer container)
    {
      _container = container;
    }

    [Test]
    public void Interfaces_without_matching_implementations_are_registered_as_MappedToType()
    {
      Assert.That(_container.Registrations.Any(r => r.MappedToType == typeof(IWillNotHaveARegistration)), Is.True);
    }

    [Test]
    public void Interfaces_without_matching_implementations_are_registered_as_RegisteredType()
    {
      Assert.That(_container.Registrations.Any(r => r.RegisteredType == typeof(IWillNotHaveARegistration)), Is.True);
    }

    [Test]
    public void Classes_without_matching_interface_are_registered_as_MappedToType()
    {
      Assert.That(
        _container.Registrations.Any(r => r.MappedToType == typeof(ClassesWithoutAnInterfaceAreNotRegistered)),
        Is.True);
    }

    [Test]
    public void Classes_without_matching_interface_are_not_registered_as_RegisteredType()
    {
      Assert.That(
        _container.Registrations.Any(r => r.RegisteredType == typeof(ClassesWithoutAnInterfaceAreNotRegistered)),
        Is.True);
    }

  }
}