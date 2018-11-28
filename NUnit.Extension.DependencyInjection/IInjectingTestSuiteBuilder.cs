using System.Collections.Generic;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace NUnit.Extension.DependencyInjection
{
  public interface IInjectingTestSuiteBuilder
  {
    IEnumerable<TestSuite> Build(ITypeInfo typeInfo);
  }
}