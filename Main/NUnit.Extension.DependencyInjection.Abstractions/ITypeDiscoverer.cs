// Copyright (c) Kaleb Pederson Software LLC. All rights reserved.
// Licensed under the MIT license. See LICENSE file alongside the solution file for full license information.

namespace NUnit.Extension.DependencyInjection.Abstractions
{
  /// <summary>
  /// Identifies and registers types with the inversion of control container.
  /// </summary>
  public interface ITypeDiscoverer
  {
    /// <summary>
    /// Identifies and registers types with the inversion of control container.
    /// The types may be discovered through assembly scanning or some other
    /// process.
    /// </summary>
    /// <param name="container">
    /// The container into which the discovered types should be registered.
    /// </param>
    /// <exception cref="TypeDiscoveryException">
    /// Thrown when an error is encountered in the type discovery and registration
    /// process.
    /// </exception>
    void Discover(object container);
  }
}