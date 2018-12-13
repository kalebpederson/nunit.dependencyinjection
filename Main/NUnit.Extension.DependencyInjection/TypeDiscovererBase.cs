﻿namespace NUnit.Extension.DependencyInjection
{
  /// <summary>
  /// Convenience class for adding strong typing to the <see
  /// cref="ITypeDiscoverer"/> implementations.
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