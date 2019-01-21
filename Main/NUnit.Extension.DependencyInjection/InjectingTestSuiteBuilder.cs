// Copyright (c) Kaleb Pederson Software LLC. All rights reserved.
// Licensed under the MIT license. See LICENSE file alongside the solution file for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;

namespace NUnit.Extension.DependencyInjection
{
  /// <summary>
  /// Builder class that performs dependency injection.
  /// </summary>
  public class InjectingTestSuiteBuilder : IInjectingTestSuiteBuilder
  {
    private static readonly IPreFilter _emptyPreFilter = new AlwaysTruePreFilter();

    private readonly Lazy<IInjectionFactory> _lazyInjectionFactory;
    private readonly NUnitTestFixtureBuilder _builder = new NUnitTestFixtureBuilder();

    /// <summary>
    /// Creates an instance of the builder using the provided parameters.
    /// </summary>
    /// <param name="injectionFactoryTypeSelector">
    /// Selects the type of the injection factory.
    /// </param>
    /// <param name="typeDiscovererTypeSelector">
    /// Selects the type of the type discoverer.
    /// </param>
    public InjectingTestSuiteBuilder(
      IInjectionFactoryTypeSelector injectionFactoryTypeSelector,
      ITypeDiscovererTypeSelector typeDiscovererTypeSelector
      )
    {
      _lazyInjectionFactory = new Lazy<IInjectionFactory>(
        () =>
        {
          var factory = CreateInjectionFactoryFromType(injectionFactoryTypeSelector.GetInjectionType());
          factory.Initialize(CreateTypeDiscovererFromType(typeDiscovererTypeSelector.GetTypeDiscovererType()));
          return factory;
        },
        true);
    }

    /// <inheritdoc />
    public IEnumerable<TestSuite> Build(ITypeInfo typeInfo)
    {
      return CreateTestSuite(typeInfo, GetParametersFor(typeInfo.Type).FirstOrDefault());
    }

    /// <summary>
    /// Returns a set of <see cref="ITestFixtureData"/> items for use as arguments
    /// to a parameterized test fixture.
    /// </summary>
    /// <param name="sourceType">The type for which data is needed.</param>
    /// <returns>Parameters needed to create the test fixture.</returns>
    // ReSharper disable once MemberCanBeProtected.Global
    protected internal virtual IEnumerable<ITestFixtureData> GetParametersFor(Type sourceType)
    {
      var source = GetTestFixtureSource(sourceType);
      var injectionParameters = source.GetInjectionParameters();
      return new [] {
        new TestFixtureParameters(injectionParameters)
        {
          // set the test name so that it doesn't show up as
          // a parameterized test.
          TestName = TypeHelper.GetDisplayName(sourceType)
        }
      };
    }

    /// <summary>
    /// Creates the test suite for the type described by <paramref name="typeInfo"/> which
    /// requires dependencies specified by <paramref name="testFixtureData"/> to be injected.
    /// </summary>
    /// <param name="typeInfo">The type into which dependencies are injected.</param>
    /// <param name="testFixtureData">The parameters required by the <paramref name="typeInfo"/>.</param>
    /// <returns>
    /// A non-null test suite.
    /// </returns>
    protected virtual IEnumerable<TestSuite> CreateTestSuite(
      ITypeInfo typeInfo,
      ITestFixtureData testFixtureData)
    {
      yield return _builder.BuildFrom(typeInfo, _emptyPreFilter, testFixtureData);
    }

    private InjectionArgsSource GetTestFixtureSource(Type sourceType)
    {
      return new InjectionArgsSource(t => _lazyInjectionFactory.Value.Create(t), sourceType);
    }

    private static IInjectionFactory CreateInjectionFactoryFromType(Type injectionFactoryType)
    {
      try
      {
        return (IInjectionFactory) Reflect.Construct(injectionFactoryType);
      }
      catch (Exception ex)
      {
        throw new ArgumentException(
          $"Unable to create {typeof(IInjectionFactory)} of type {injectionFactoryType.FullName}", ex);
      }
    }

    private static ITypeDiscoverer CreateTypeDiscovererFromType(Type typeDiscovererType)
    {
      try
      {
        return (ITypeDiscoverer) Reflect.Construct(typeDiscovererType);
      }
      catch (Exception ex)
      {
        throw new ArgumentException(
          $"Unable to create {typeof(ITypeDiscoverer).FullName} of type {typeDiscovererType.FullName}", ex
        );
      }
    }

    private class AlwaysTruePreFilter : IPreFilter
    {
      public bool IsMatch(Type type) => true;
      public bool IsMatch(Type type, MethodInfo method) => true;
    }
  }
}