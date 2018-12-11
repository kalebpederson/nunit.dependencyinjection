using System;

namespace NUnit.Extension.DependencyInjection
{
  /// <summary>
  /// Interface by which the <see cref="IInjectionFactory" /> is selected to
  /// be used to perform dependency injection on the parameters injected into
  /// the test fixtures.
  /// </summary>
  public interface IInjectionFactoryTypeSelector
  {
    /// <summary>
    /// Identifies the type implementing <see cref="IInjectionFactory"/> that
    /// will be used to perform dependency injection.
    /// </summary>
    /// <returns>The type used to perform dependency injection.</returns>
    Type GetInjectionType();
  }
}