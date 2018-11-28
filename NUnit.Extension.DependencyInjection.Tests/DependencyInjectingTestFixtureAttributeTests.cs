using System;
using System.Collections;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace NUnit.Extension.DependencyInjection.Tests
{
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
  
  
  [TestFixture]
  public class DependencyInjectingTestFixtureAttributeTests
  {
    internal class ValidIocContainer : IIocContainer
    {
      public static Func<Type, object> FactoryFunc = null;
      
      /// <inheritdoc />
      public object Create(Type type)
      {
        return FactoryFunc(type);
      }
    }

    [Test]
    public void Ctor_throws_nothing_if_no_arguments_provided()
    {
      Assert.That(
        () => new DependencyInjectingTestFixtureAttribute(null),
        Throws.Nothing
        );
    }

    [Test]
    public void BuildFrom_throws_an_ArgumentOutOfRangeException_if_factoryType_does_not_implement_IInjectionFactory()
    {
      var attr = new DependencyInjectingTestFixtureAttribute(typeof(IEnumerable));
      Assert.That(
        () => attr.BuildFrom(new TypeWrapper(typeof(IEnumerable))),
        Throws.InstanceOf<ArgumentOutOfRangeException>()
        );
    }

    [Test]
    public void BuildFrom_does_not_throw_for_valid_type_implementing_IInjectionFactory()
    {
      var attr = new DependencyInjectingTestFixtureAttribute(typeof(ValidIocContainer));
      Assert.That(
        () => attr.BuildFrom(new TypeWrapper(typeof(ValidIocContainer))),
        Throws.Nothing
        );
    }

    [TestCase(typeof(TestWithSingleObjectDependency))]
    [TestCase(typeof(TestWithTwoObjectDependency))]
    public void BuildFrom_returns_a_single_TestSuite_named_after_the_test_class(Type type)
    {
      ValidIocContainer.FactoryFunc = t => new object();
      var attr = new DependencyInjectingTestFixtureAttribute(typeof(ValidIocContainer));
      var suite = attr.BuildFrom(new TypeWrapper(type));
      Assert.That(suite, Is.Not.Null);
      Assert.That(suite.Count(), Is.EqualTo(1));
      Assert.That(suite.First().Name, Is.EqualTo(type.Name));
    }
  }
}