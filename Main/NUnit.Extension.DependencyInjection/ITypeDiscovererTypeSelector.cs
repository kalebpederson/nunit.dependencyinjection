// Copyright (c) Kaleb Pederson Software LLC. All rights reserved.
// Licensed under the MIT license. See LICENSE file alongside the solution file for full license information.

using System;

namespace NUnit.Extension.DependencyInjection
{
  /// <summary>
  /// Interface for identifying the <see cref="ITypeDiscoverer"/> that
  /// should be used to register types with the inversion of control
  /// container.
  /// </summary>
  public interface ITypeDiscovererTypeSelector
  {
    /// <summary>
    /// Identifies the type of the <see cref="ITypeDiscoverer"/>.
    /// </summary>
    /// <returns>The type of the <see cref="ITypeDiscoverer"/>.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when when the application is not properly configured for
    /// type discovery, such as when no <see
    /// cref="NUnitTypeDiscovererAttribute"/> is present.
    /// </exception>
    Type GetTypeDiscovererType();
    
    /// <summary>
    /// Provides the arguments required for the type <see cref="ITypeDiscoverer"/>
    /// that is being used.
    /// </summary>
    /// <returns>
    /// The arguments required for the creation of the <see cref="ITypeDiscoverer"/>.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when when the application is not properly configured for
    /// type discovery, such as when no <see cref="NUnitTypeDiscovererAttribute"/>
    /// is present.
    /// </exception>
    object[] GetTypeDiscovererArguments();
  }
}