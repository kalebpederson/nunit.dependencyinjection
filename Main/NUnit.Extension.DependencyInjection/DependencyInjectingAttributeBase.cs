using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace NUnit.Extension.DependencyInjection
{
  [AttributeUsage(AttributeTargets.Class)]
  public class DependencyInjectingAttributeBase : NUnitAttribute, IFixtureBuilder
  {
    private readonly Lazy<IInjectingTestSuiteBuilder> _lazyBuilder;

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