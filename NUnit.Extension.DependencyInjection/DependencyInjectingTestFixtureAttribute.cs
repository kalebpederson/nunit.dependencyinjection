using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;

namespace NUnit.Extension.DependencyInjection
{
  /// <summary>
  /// Identifies a type as needing dependency injection.
  /// </summary>
  /// <remarks>
  /// Shamelessly adapted from the <see cref="TestFixtureSourceAttribute"/>.
  /// </remarks>
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
  public class DependencyInjectingTestFixtureAttribute : NUnitAttribute, IFixtureBuilder2
  {
    private static readonly IPreFilter _emptyPreFilter = new AlwaysTruePreFilter();
    
    private readonly Lazy<IInjectionFactory> _lazyInjectionFactory;
    private readonly NUnitTestFixtureBuilder _builder = new NUnitTestFixtureBuilder();

    public DependencyInjectingTestFixtureAttribute() : this(GetInjectionFactoryTypeFromAttribute())
    {
    }
    
    public DependencyInjectingTestFixtureAttribute(Type factoryType)
    {
      InjectionFactoryTypeValidator.AssertIsValidFactoryType(factoryType);
      _lazyInjectionFactory = new Lazy<IInjectionFactory>(
        () => CreateInjectionFactoryFromType(factoryType),
        true
        );
    }
    
    /// <summary>
    /// Builds any number of test fixtures from the specified type.
    /// </summary>
    /// <param name="typeInfo">The TypeInfo for which fixtures are to be constructed.</param>
    public IEnumerable<TestSuite> BuildFrom(ITypeInfo typeInfo)
    {
      return BuildFrom(typeInfo, _emptyPreFilter);
    }

    /// <summary>
    /// Builds any number of test fixtures from the specified type.
    /// </summary>
    /// <param name="typeInfo">The TypeInfo for which fixtures are to be constructed.</param>
    /// <param name="filter">PreFilter used to select methods as tests.</param>
    public IEnumerable<TestSuite> BuildFrom(ITypeInfo typeInfo, IPreFilter filter)
    {
      Type sourceType = typeInfo.Type;
      yield return _builder.BuildFrom(typeInfo, filter, GetParametersFor(sourceType).FirstOrDefault());
    }

    /// <summary>
    /// Returns a set of ITestFixtureData items for use as arguments
    /// to a parameterized test fixture.
    /// </summary>
    /// <param name="sourceType">The type for which data is needed.</param>
    /// <returns>Parameters needed to create the test fixture.</returns>
    public IEnumerable<ITestFixtureData> GetParametersFor(Type sourceType)
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

    internal InjectionArgsSource GetTestFixtureSource(Type sourceType)
    {
      return new InjectionArgsSource(t => _lazyInjectionFactory.Value.Create(t), sourceType);
    }

    internal static Type GetInjectionFactoryTypeFromAttribute()
    {
      var injectionFactoryAttribute = AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(a => a.GetCustomAttributes(typeof(NUnitTypeInjectionFactoryAttribute), false))
        .OfType<NUnitTypeInjectionFactoryAttribute>()
        .FirstOrDefault();
      if (injectionFactoryAttribute == null)
      {
        throw new InvalidOperationException(
          $"{nameof(DependencyInjectingTestFixtureAttribute)} requires an injection plugin be loaded. Please ensure " +
          $"that one is present or create one using the {typeof(IInjectionFactory).FullName} interface " +
          $"and register it using the {typeof(NUnitTypeInjectionFactoryAttribute).FullName} attribute.");
      }
      InjectionFactoryTypeValidator.AssertIsValidFactoryType(injectionFactoryAttribute.InjectionFactoryType);
      Trace.TraceInformation(
        $"Found {nameof(NUnitTypeInjectionFactoryAttribute)} in assembly " +
        $"{injectionFactoryAttribute.GetType().Assembly.FullName}. Will use the {injectionFactoryAttribute.InjectionFactoryType} type " +
        "to create dependencies.");
      return injectionFactoryAttribute.InjectionFactoryType;
    }

    private static IInjectionFactory CreateInjectionFactoryFromType(Type injectionFactoryType)
    {
      return (IInjectionFactory)Reflect.Construct(injectionFactoryType);
    }

    internal class AlwaysTruePreFilter : IPreFilter
    {
      public bool IsMatch(Type type) => true;
      public bool IsMatch(Type type, MethodInfo method) => true;
    }
  }
}