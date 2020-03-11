using System.Linq;
using NUnit.Extension.DependencyInjection;
using NUnit.Framework;
using Unity;

namespace ConventionMappingTypeDiscovererTests
{
  public interface IWillNotHaveARegistration {}
  public class ClassesWithoutAnInterfaceAreNotRegistered {}

  public interface IWillHaveARegistration {}
  public class WillHaveARegistration : IWillHaveARegistration {}
  
  [DependencyInjectingTestFixture]
  public class RegistrationTests
  {
    private readonly IUnityContainer _container;

    public RegistrationTests(IUnityContainer container)
    {
      _container = container;
    }

    [Test]
    public void Interfaces_without_matching_implementations_are_not_registered_as_MappedToType()
    {
      Assert.That(
        _container.Registrations.Any(r => r.MappedToType == typeof(IWillNotHaveARegistration)),
        Is.False,
        "Found a registration with a MappedToType for an interface not accompanied by a concrete type.");
    }

    [Test]
    public void Interfaces_without_matching_implementations_are_not_registered_as_RegisteredType()
    {
      Assert.That(
        _container.Registrations.Any(r => r.RegisteredType == typeof(IWillNotHaveARegistration)),
        Is.False,
        "Found a registration with a RegisteredType for an interface not accompanied by a concrete type.");
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

    [Test]
    public void Classes_with_matching_interface_are_registered_with_expected_mapping()
    {
      Assert.That(
        _container.Registrations.Any(r =>
          r.RegisteredType == typeof(IWillHaveARegistration) 
          && r.MappedToType == typeof(WillHaveARegistration)),
        Is.True);
    }
    
    
  }
}