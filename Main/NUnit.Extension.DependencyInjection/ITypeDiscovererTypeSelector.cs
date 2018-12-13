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
    Type GetTypeDiscovererType();
  }
}