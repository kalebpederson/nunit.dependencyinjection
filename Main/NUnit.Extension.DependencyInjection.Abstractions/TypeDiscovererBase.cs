// Copyright (c) Kaleb Pederson Software LLC. All rights reserved.
// Licensed under the MIT license. See LICENSE file alongside the solution file for full license information.

namespace NUnit.Extension.DependencyInjection.Abstractions
{
  /// <summary>
  /// Convenience class for adding strong typing to the <see
  /// cref="NUnit.Extension.DependencyInjection.Abstractions.ITypeDiscoverer"/> implementations.
  /// </summary>
  /// <typeparam name="T">
  /// The type of the inversion of control container.
  /// </typeparam>
  public abstract class TypeDiscovererBase<T> : ITypeDiscoverer
  {
    /// <inheritdoc />
    public void Discover(object container)
    {
      DiscoverInternal((T) container);
    }

    /// <summary>
    /// Convenience method by which type discoverers can be easily added
    /// and/or run against a strongly typed instance of the container.
    /// </summary>
    /// <param name="container">
    /// The inversion of control container instance against which the
    /// type discoverers will be run.
    /// </param>
    protected abstract void DiscoverInternal(T container);
  }
}