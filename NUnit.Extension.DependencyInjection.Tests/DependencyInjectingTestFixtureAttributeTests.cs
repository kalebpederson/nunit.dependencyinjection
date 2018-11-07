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
    internal class ValidInjectionFactory : IInjectionFactory
    {
      public static Func<Type, object> FactoryFunc = null;
      
      /// <inheritdoc />
      public object Create(Type type)
      {
        return FactoryFunc(type);
      }
    }

    [Test]
    public void Ctor_throws_an_ArgumentNullException_if_factoryType_is_null()
    {
      Assert.That(
        () => new DependencyInjectingTestFixtureAttribute(null),
        Throws.InstanceOf<ArgumentNullException>()
        );
    }

    [Test]
    public void Ctor_throws_an_ArgumentOutOfRangeException_if_factoryType_does_not_implement_IInjectionFactory()
    {
      Assert.That(
        () => new DependencyInjectingTestFixtureAttribute(typeof(IEnumerable)),
        Throws.InstanceOf<ArgumentOutOfRangeException>()
        );
    }

    [Test]
    public void Ctor_does_not_throw_for_valid_type_implementing_IInjectionFactory()
    {
      Assert.That(
        () => new DependencyInjectingTestFixtureAttribute(typeof(ValidInjectionFactory)),
        Throws.Nothing
        );
    }

    [Test]
    public void BuildFrom_throws_InvalidOperationException_when_no_factory_is_registered()
    {
      DependencyInjectingTestFixtureAttribute attr = null;
      Assert.That(
        () => attr = new DependencyInjectingTestFixtureAttribute(),
        Throws.InstanceOf<InvalidOperationException>()
        );
    }

    [TestCase(typeof(TestWithSingleObjectDependency))]
    [TestCase(typeof(TestWithTwoObjectDependency))]
    public void BuildFrom_returns_a_single_TestSuite_named_after_the_test_class(Type type)
    {
      ValidInjectionFactory.FactoryFunc = t => new object();
      var attr = new DependencyInjectingTestFixtureAttribute(typeof(ValidInjectionFactory));
      var suite = attr.BuildFrom(new TypeWrapper(type));
      Assert.That(suite, Is.Not.Null);
      Assert.That(suite.Count(), Is.EqualTo(1));
      Assert.That(suite.First().Name, Is.EqualTo(type.Name));
    }
    
    [Test]
    public void GetParametersFor_returns_TestFixtureParameters_with_injected_instances()
    {
      var i = 0;
      var objects = new [] {new object(), new object()};
      ValidInjectionFactory.FactoryFunc = t => objects[i++ % 2];
      var attr = new DependencyInjectingTestFixtureAttribute(typeof(ValidInjectionFactory));
      var suite = attr.GetParametersFor(typeof(TestWithTwoObjectDependency));
      Assert.That(suite, Is.Not.Null);
      Assert.That(suite.Count(), Is.EqualTo(1));
      var testFixtureData = suite.First();
      Assert.That(testFixtureData.TestName, Is.EqualTo(nameof(TestWithTwoObjectDependency)));
      Assert.That(testFixtureData.Arguments, Is.EqualTo(objects));
    }
    
  }
}