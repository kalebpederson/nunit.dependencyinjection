﻿// Copyright (c) Kaleb Pederson Software LLC. All rights reserved.
// Licensed under the MIT license. See LICENSE file alongside the solution file for full license information.

using System.Collections.Generic;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace NUnit.Extension.DependencyInjection
{
  /// <summary>
  /// Interface used to create the test suite or suites on which dependency
  /// injection will be performed if necessary.
  /// </summary>
  /// <remarks>
  /// This interface is largely the dependency injection workhorse, providing
  /// the functionality necessary to create a test suite, and thereby the
  /// instance of the test fixture, while injecting necessary parameters.
  /// </remarks>
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