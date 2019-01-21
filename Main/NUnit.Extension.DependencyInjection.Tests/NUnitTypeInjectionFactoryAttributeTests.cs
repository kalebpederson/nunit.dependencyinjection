// Copyright (c) Kaleb Pederson Software LLC. All rights reserved.
// Licensed under the MIT license. See LICENSE file alongside the solution file for full license information.

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
          .With.Message.Match($"must be of type {nameof(IInjectionFactory)}")
        );
    }

    [Test]
    public void Ctor_does_not_throw_for_valid_type_implementing_IInjectionFactory()
    {
      Assert.That(
        () => new NUnitTypeInjectionFactoryAttribute(typeof(DependencyInjectingTestFixtureAttributeTests.ValidInjectionFactory)),
        Throws.Nothing
        );
    }

    [Test]
    public void Ctor_throws_foo_when_no_public_noargs_constructor_is_present()
    {
      Assert.That(
        () => new NUnitTypeInjectionFactoryAttribute(typeof(InvalidInjectionFactory)),
        Throws.InstanceOf<ArgumentOutOfRangeException>()
          .With.Message.Match("public no-args constructor")
        );
    }

    private class InvalidInjectionFactory : IInjectionFactory
    {
      public static readonly Func<Type, object> FactoryFunc = null;

      public InvalidInjectionFactory(object badDependency)
      {
      }

      /// <inheritdoc />
      public void Initialize(ITypeDiscoverer typeDiscoverer)
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
