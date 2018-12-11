using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;

namespace NUnit.Extension.DependencyInjection
{
  public class InjectingTestSuiteBuilder : IInjectingTestSuiteBuilder
  {
    private static readonly IPreFilter EmptyPreFilter = new AlwaysTruePreFilter();

    private readonly Lazy<IInjectionFactory> _lazyInjectionFactory;
    private readonly NUnitTestFixtureBuilder _builder = new NUnitTestFixtureBuilder();

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
          $"Unable to create {typeof(IInjectionFactory)} of type {injectionFactoryType.FullName}", ex
        );
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
    
    /// <summary>
    /// Returns a set of <see cref="ITestFixtureData"/> items for use as arguments
    /// to a parameterized test fixture.
    /// </summary>
    /// <param name="sourceType">The type for which data is needed.</param>
    /// <returns>Parameters needed to create the test fixture.</returns>
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
        }};
    }

    protected virtual IEnumerable<TestSuite> CreateTestSuite(
      ITypeInfo typeInfo,
      ITestFixtureData testFixtureData)
    {
      yield return _builder.BuildFrom(typeInfo, EmptyPreFilter, testFixtureData);
    }

    private class AlwaysTruePreFilter : IPreFilter
    {
      public bool IsMatch(Type type) => true;
      public bool IsMatch(Type type, MethodInfo method) => true;
    }
  }
}