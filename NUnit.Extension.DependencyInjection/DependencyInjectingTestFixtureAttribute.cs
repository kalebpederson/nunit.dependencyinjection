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

    public DependencyInjectingTestFixtureAttribute(Type injectionFactoryType, Type typeSelectorType)
      : base(StaticTypeBasedTestSuiteBuilderFactory(injectionFactoryType, typeSelectorType))
    {
    }

    private static Func<IInjectingTestSuiteBuilder> StaticTypeBasedTestSuiteBuilderFactory(
      Type injectionFactoryType, Type typeSelectorType
      )
    {
      return () => new InjectingTestSuiteBuilder(
        new StaticInjectionFactoryTypeSelector(injectionFactoryType), 
        new StaticTypeDiscovererTypeSelector(typeSelectorType));
    }

    private static IInjectingTestSuiteBuilder AttributeBasedTestSuiteBuilderFactory()
    {
      return new InjectingTestSuiteBuilder(
        new AttributeBasedInjectionFactoryTypeSelector(),
        new AttributeBasedTypeDiscovererTypeSelector()
        );
    }
  }
}