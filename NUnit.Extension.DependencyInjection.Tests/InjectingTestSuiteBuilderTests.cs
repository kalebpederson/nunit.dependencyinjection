using System.Linq;
using NUnit.Framework;

namespace NUnit.Extension.DependencyInjection.Tests
{
  [TestFixture]
  public class InjectingTestSuiteBuilderTests
  {
    [Test]
    public void GetParametersFor_returns_TestFixtureParameters_with_injected_instances()
    {
      var i = 0;
      var objects = new [] {new object(), new object()};
      DependencyInjectingTestFixtureAttributeTests.ValidInjectionFactory.FactoryFunc = t => objects[i++ % 2];
      var attr = new InjectingTestSuiteBuilder(
        new StaticInjectionFactoryTypeSelector(
          typeof(DependencyInjectingTestFixtureAttributeTests.ValidInjectionFactory)), 
        new StaticTypeDiscovererTypeSelector(
          typeof(DependencyInjectingTestFixtureAttributeTests.ValidTypeDiscoverer))
        );
      var suite = attr.GetParametersFor(typeof(TestWithTwoObjectDependency));
      Assert.That(suite, Is.Not.Null);
      Assert.That(suite.Count(), Is.EqualTo(1));
      var testFixtureData = suite.First();
      Assert.That(testFixtureData.TestName, Is.EqualTo(nameof(TestWithTwoObjectDependency)));
      Assert.That(testFixtureData.Arguments, Is.EqualTo(objects));
    }
    
  }
}
