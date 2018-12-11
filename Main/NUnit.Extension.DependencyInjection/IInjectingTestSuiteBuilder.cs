using System.Collections.Generic;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace NUnit.Extension.DependencyInjection
{
  /// <summary>
  /// Interface used to create the test suite or suites on which dependency
  /// injection will be performed if necessary.
  /// </summary>
  public interface IInjectingTestSuiteBuilder
  {
    /// <summary>
    /// Creates the test suite or suites on which dependency injection will be
    /// performed if necessary.
    /// </summary>
    /// <param name="typeInfo">
    /// Information about the type of suite to be created and into which
    /// dependency injection is likely needed.
    /// </param>
    /// <returns>One or more test suites.</returns>
    IEnumerable<TestSuite> Build(ITypeInfo typeInfo);
  }
}