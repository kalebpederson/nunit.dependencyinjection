﻿// Copyright (c) Kaleb Pederson Software LLC. All rights reserved.
// Licensed under the MIT license. See LICENSE file alongside the solution file for full license information.

using System;
using System.Collections.Generic;
using NUnit.Extension.DependencyInjection.Abstractions;
using NUnit.Framework;

namespace NUnit.Extension.DependencyInjection.Tests
{
  [TestFixture]
  public class InjectionArgsSourceTests
  {
    private interface IDependency1
    {
    }

    private interface IDependency2
    {
    }

    private class Dependency1 : IDependency1
    {
    }
    private class Dependency2 : IDependency2
    {
    }

    private class DependencyInjectingTestClass
    {
      public DependencyInjectingTestClass(IDependency1 dependency1, IDependency2 dependency2)
      {
      }
    }

    private class MyGenericTest<T>
    {
      public MyGenericTest(IDependency1 dependency1)
      {
      }
    }

    private class TestClassWithPrivateConstructor
    {
      private TestClassWithPrivateConstructor()
      {
      }
    }

    private class TestClassWithTooManyConstructors
    {
      public TestClassWithTooManyConstructors(IDependency1 dep1)
      {
      }

      public TestClassWithTooManyConstructors(IDependency2 dep2)
      {
      }
    }

    private class TestClassWithNoConstructorParameters
    {
    }

    [Test]
    public void InjectionArgsSource_creates_argument_list_with_values_returned_by_factory()
    {
      var dep1 = new Dependency1();
      var dep2 = new Dependency2();

      object Factory(Type t)
      {
        if (typeof(IDependency1) == t)
        {
          return dep1;
        }

        if (typeof(IDependency2) == t)
        {
          return dep2;
        }

        throw new Exception($"Requested unsupported dependency type {t.FullName}");
      }

      var source = new InjectionArgsSource<DependencyInjectingTestClass>(Factory);

      var contents = source.GetInjectionParameters();
      Assert.That(contents.Length, Is.EqualTo(2));
      Assert.That(contents[0], Is.InstanceOf<IDependency1>());
      Assert.That(contents[0], Is.InstanceOf<Dependency1>());
      Assert.That(contents[0], Is.SameAs(dep1));
      Assert.That(contents[1], Is.InstanceOf<IDependency2>());
      Assert.That(contents[1], Is.InstanceOf<Dependency2>());
      Assert.That(contents[1], Is.SameAs(dep2));
    }

    [Test]
    public void GetInjectionParameters_throws_a_DependencyResolutionException_if_the_type_cannot_be_resolved()
    {
      object Factory(Type t) => throw new Exception($"Requested unsupported dependency type {t.FullName}");
      var source = new InjectionArgsSource<DependencyInjectingTestClass>(Factory);

      var thrown = Assert.Throws<DependencyResolutionException>(() => source.GetInjectionParameters());
      Assert.That(thrown.InjectionClassType, Is.EqualTo(typeof(DependencyInjectingTestClass)));
      Assert.That(thrown.InjectionParameterType, Is.EqualTo(typeof(IDependency1)));
    }

    [Test]
    public void Ctor_throws_an_ArgumentNullException_if_the_injection_factory_is_null()
    {
      const Func<Type, object> nullFactory = null;
      Assert.That(
        () => new InjectionArgsSource<DependencyInjectingTestClass>(nullFactory),
        Throws.InstanceOf<ArgumentNullException>()
        );
    }

    [Test]
    public void Ctor_throws_ArgumentOutOfRangeException_if_the_injection_type_does_not_have_a_public_constructor()
    {
      object Factory(Type t) => new object();
      Assert.That(
        () => new InjectionArgsSource<TestClassWithPrivateConstructor>(Factory),
        Throws.InstanceOf<ArgumentOutOfRangeException>()
          .With.Message.Match("single public constructor")
        );
    }

    [Test]
    public void Ctor_throws_ArgumentOutOfRangeException_if_the_injection_type_has_too_many_constructors()
    {
      object Factory(Type t) => new object();
      Assert.That(
        () => new InjectionArgsSource<TestClassWithTooManyConstructors>(Factory),
        Throws.InstanceOf<ArgumentOutOfRangeException>()
          .With.Message.Match("single public constructor")
        );
    }

    [Test]
    public void Ctor_does_not_throw_on_a_type_with_no_constructor_parameters()
    {
      object Factory(Type t) => new object();
      Assert.That(
        () => new InjectionArgsSource<TestClassWithNoConstructorParameters>(Factory),
        Throws.Nothing
        );
    }

    [TestCase(typeof(IDependency1))]
    [TestCase(typeof(IEnumerable<object>))]
    public void Ctor_throws_an_ArgumentOutOfRangeException_if_the_type_to_be_constructed_is_abstract(Type typeToInjectInto)
    {
      object Factory(Type t) => new object();
      Assert.That(
        () => new InjectionArgsSource(Factory, typeToInjectInto),
        Throws.InstanceOf<ArgumentOutOfRangeException>()
          .With.Message.Match("is an abstract type")
        );
    }

    [Test]
    public void Ctor_throws_an_ArgumentOutOfRangeException_if_the_type_to_be_constructed_is_an_open_generic_type()
    {
      object Factory(Type t) => new object();
      Assert.That(
        () => new InjectionArgsSource(Factory, typeof(MyGenericTest<>)),
        Throws.InstanceOf<ArgumentOutOfRangeException>()
          .With.Message.Match("constructable generic type")
        );
    }
  }
}