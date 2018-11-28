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

    private readonly Lazy<IIocContainer> _lazyInjectionFactory;
    private readonly NUnitTestFixtureBuilder _builder = new NUnitTestFixtureBuilder();

    public InjectingTestSuiteBuilder(
      IInjectionTypeSelector injectionTypeSelector
      )
    {
      _lazyInjectionFactory = new Lazy<IIocContainer>(
        () => CreateInjectionFactoryFromType(injectionTypeSelector.GetInjectionType()),
        true);
    }

    /// <inheritdoc />
    public IEnumerable<TestSuite> Build(ITypeInfo typeInfo)
    {
      // FIXME: this class should be responsible for orchestrating the different arguments
      // and parameters but not doing the actual work.
      return CreateTestSuite(typeInfo, GetParametersFor(typeInfo.Type).FirstOrDefault());
    }

    private InjectionArgsSource GetTestFixtureSource(Type sourceType)
    {
      return new InjectionArgsSource(t => _lazyInjectionFactory.Value.Create(t), sourceType);
    }

    private static IIocContainer CreateInjectionFactoryFromType(Type injectionFactoryType)
    {
      try
      {
        return (IIocContainer) Reflect.Construct(injectionFactoryType);
      }
      catch (Exception ex)
      {
        throw new ArgumentException(
          $"Unable to create injection factory of type {injectionFactoryType.FullName}", ex
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
      // FIXME: I need to move this method out to another class, something that
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