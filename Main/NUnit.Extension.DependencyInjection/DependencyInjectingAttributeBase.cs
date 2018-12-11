using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace NUnit.Extension.DependencyInjection
{
  /// <summary>
  /// A base class provided for convenience to support customizing the dependency injection
  /// process while using this extension.
  /// </summary>
  [AttributeUsage(AttributeTargets.Class)]
  public class DependencyInjectingAttributeBase : NUnitAttribute, IFixtureBuilder
  {
    private readonly Lazy<IInjectingTestSuiteBuilder> _lazyBuilder;

    /// <summary>
    /// Creates an instance of the base class, using the <paramref name="builderFactory"/>
    /// to create the <see cref="IInjectingTestSuiteBuilder"/> that will be used to build
    /// the test suite containing the tests.
    /// </summary>
    /// <param name="builderFactory">
    /// The factory used to create test suite.
    /// </param>
    protected DependencyInjectingAttributeBase(Func<IInjectingTestSuiteBuilder> builderFactory)
    {
      _lazyBuilder = new Lazy<IInjectingTestSuiteBuilder>(builderFactory, true);
    }
    
    /// <inheritdoc />
    public IEnumerable<TestSuite> BuildFrom(ITypeInfo typeInfo)
    {
      return GetInjectingBuilder().Build(typeInfo);
    }

    // should this be abstract?
    private IInjectingTestSuiteBuilder GetInjectingBuilder()
    {
      return _lazyBuilder.Value;
    }
  }
}