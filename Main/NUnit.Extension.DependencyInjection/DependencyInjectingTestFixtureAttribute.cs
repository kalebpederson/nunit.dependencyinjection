// Copyright (c) Kaleb Pederson Software LLC. All rights reserved.
// Licensed under the MIT license. See LICENSE file alongside the solution file for full license information.

using System;

namespace NUnit.Extension.DependencyInjection
{
  /// <summary>
  /// Attribute used to mark a test fixture as being eligible for dependency injection
  /// through the NUnit.Extension.DependencyInjection framework.
  /// </summary>
  public class DependencyInjectingTestFixtureAttribute : DependencyInjectingBaseAttribute
  {
    /// <summary>
    /// Creates an instance of the attribute. When this constructor is used dependency
    /// injection will be performed based on the types specified by the
    /// <see cref="NUnitTypeInjectionFactoryAttribute"/> and the
    /// <see cref="NUnitTypeDiscovererAttribute"/>s.
    /// </summary>
    public DependencyInjectingTestFixtureAttribute()
      : base(AttributeBasedTestSuiteBuilderFactory)
    {
    }

    /// <summary>
    /// Creates an instance of the attribute. When this constructor is used dependency
    /// injection will be performed using the types specified by the parameters.
    /// </summary>
    /// <param name="injectionFactoryType">
    /// The <see cref="IInjectionFactoryTypeSelector"/> type to be used to perform the
    /// injection.
    /// </param>
    /// <param name="typeSelectorType">
    /// The <see cref="ITypeDiscovererTypeSelector"/> type to be used to discover the
    /// types to be registered into the inversion of control container.
    /// </param>
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