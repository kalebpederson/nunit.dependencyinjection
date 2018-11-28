using System;

namespace NUnit.Extension.DependencyInjection
{
  public class DependencyInjectingTestFixtureAttribute : DependencyInjectingAttributeBase
  {
    /// <inheritdoc />
    public DependencyInjectingTestFixtureAttribute() 
      : base(AttributeBasedTestSuiteBuilderFactory)
    {
    }

    public DependencyInjectingTestFixtureAttribute(Type type)
      : base(StaticTypeBasedTestSuiteBuilderFactory(type))
    {
    }

    private static Func<IInjectingTestSuiteBuilder> StaticTypeBasedTestSuiteBuilderFactory(Type type)
    {
      return () => new InjectingTestSuiteBuilder(new StaticInjectionTypeSelector(type));
    }

    private static IInjectingTestSuiteBuilder AttributeBasedTestSuiteBuilderFactory()
    {
      return new InjectingTestSuiteBuilder(new AttributeBasedInjectionTypeSelector());
    }
  }
}