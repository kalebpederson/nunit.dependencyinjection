using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace NUnit.Extension.DependencyInjection.Tests
{
  [TestFixture]
  public class NUnitTypeInjectionFactoryAttributeTests
  {
    [Test]
    public void Ctor_throws_an_ArgumentNullException_if_factoryType_is_null()
    {
      Assert.That(
        () => new NUnitTypeInjectionFactoryAttribute(null), 
        Throws.InstanceOf<ArgumentNullException>()
        );
    }

    [Test]
    public void Ctor_throws_an_ArgumentOutOfRangeException_if_factoryType_does_not_implement_IInjectionFactory()
    {
      Assert.That(
        () => new NUnitTypeInjectionFactoryAttribute(typeof(IEnumerable<>)), 
        Throws.InstanceOf<ArgumentOutOfRangeException>()
          .With.Message.Match($"must be of type {nameof(IIocContainer)}")
        );
    }

    [Test]
    public void Ctor_does_not_throw_for_valid_type_implementing_IInjectionFactory()
    {
      Assert.That(
        () => new NUnitTypeInjectionFactoryAttribute(typeof(DependencyInjectingTestFixtureAttributeTests.ValidIocContainer)), 
        Throws.Nothing
        );
    }

    [Test]
    public void Ctor_throws_foo_when_no_public_noargs_constructor_is_present()
    {
      Assert.That(
        () => new NUnitTypeInjectionFactoryAttribute(typeof(InvalidIocContainer)), 
        Throws.InstanceOf<ArgumentOutOfRangeException>()
          .With.Message.Match("public no-args constructor")
        );
    }
    
    internal class InvalidIocContainer : IIocContainer
    {
      public static Func<Type, object> FactoryFunc = null;

      public InvalidIocContainer(object badDependency)
      {
      }
      
      /// <inheritdoc />
      public object Create(Type type)
      {
        return FactoryFunc(type);
      }
    }
    
  }
}