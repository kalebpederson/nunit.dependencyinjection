// Copyright (c) Kaleb Pederson Software LLC. All rights reserved.
// Licensed under the MIT license. See LICENSE file alongside the solution file for full license information.

using System;

namespace NUnit.Extension.DependencyInjection.Abstractions
{
  /// <summary>
  /// Interface by which the <see cref="NUnit.Extension.DependencyInjection.Abstractions.IInjectionFactory" /> is selected to
  /// be used to perform dependency injection on the parameters injected into
  /// the test fixtures.
  /// </summary>
  public interface IInjectionFactoryTypeSelector
  {
    /// <summary>
    /// Identifies the type implementing <see cref="NUnit.Extension.DependencyInjection.Abstractions.IInjectionFactory"/> that
    /// will be used to perform dependency injection.
    /// </summary>
    /// <returns>The type used to perform dependency injection.</returns>
    Type GetInjectionType();
  }
}