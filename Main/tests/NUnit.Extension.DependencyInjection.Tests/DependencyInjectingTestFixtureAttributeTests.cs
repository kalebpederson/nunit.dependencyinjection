// Copyright (c) Kaleb Pederson Software LLC. All rights reserved.
// Licensed under the MIT license. See LICENSE file alongside the solution file for full license information.

using System;
using System.Collections;
using System.Linq;
using NUnit.Extension.DependencyInjection.Abstractions;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace NUnit.Extension.DependencyInjection.Tests
{
  [TestFixture]
  public class DependencyInjectingTestFixtureAttributeTests
  {
    internal class ValidInjectionFactory : IInjectionFactory
    {
      public static Func<Type, object> FactoryFunc { get; set; }

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

    internal class ValidTypeDiscoverer : ITypeDiscoverer
    {
      /// <inheritdoc />
      public void Discover(object container)
      {
      }
    }

    [Test]
    public void Ctor_throws_nothing_if_no_arguments_provided()
    {
      Assert.That(
        () => new DependencyInjectingTestFixtureAttribute(),
        Throws.Nothing
        );
    }

    [Test]
    public void BuildFrom_throws_an_ArgumentOutOfRangeException_if_factoryType_does_not_implement_IInjectionFactory()
    {
      var attr = new DependencyInjectingTestFixtureAttribute(typeof(IEnumerable), typeof(ValidTypeDiscoverer));
      Assert.That(
        () => attr.BuildFrom(new TypeWrapper(typeof(IEnumerable))),
        Throws.InstanceOf<ArgumentOutOfRangeException>()
        );
    }

    [Test]
    public void BuildFrom_does_not_throw_for_valid_type_implementing_IInjectionFactory()
    {
      var attr = new DependencyInjectingTestFixtureAttribute(typeof(ValidInjectionFactory), typeof(ValidTypeDiscoverer));
      Assert.That(
        () => attr.BuildFrom(new TypeWrapper(typeof(ValidInjectionFactory))),
        Throws.Nothing
        );
    }

    [TestCase(typeof(TestWithSingleObjectDependency))]
    [TestCase(typeof(TestWithTwoObjectDependency))]
    public void BuildFrom_returns_a_single_TestSuite_named_after_the_test_class(Type type)
    {
      ValidInjectionFactory.FactoryFunc = t => new object();
      var attr = new DependencyInjectingTestFixtureAttribute(typeof(ValidInjectionFactory), typeof(ValidTypeDiscoverer));
      var suite = attr.BuildFrom(new TypeWrapper(type)).ToList();
      Assert.That(suite, Is.Not.Null);
      Assert.That(suite.Count, Is.EqualTo(1));
      Assert.That(suite.First().Name, Is.EqualTo(type.Name));
    }
  }

  public class TestWithSingleObjectDependency
  {
    public TestWithSingleObjectDependency(object obj)
    {
    }
  }

  public class TestWithTwoObjectDependency
  {
    public TestWithTwoObjectDependency(object obj, object obj2)
    {
    }
  }
}