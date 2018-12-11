using System;

namespace NUnit.Extension.DependencyInjection
{
  /// <summary>
  /// The main interface used to perform dependency injection on test fixtures.
  /// </summary>
  public interface IInjectionFactory
  {
    // TODO: Should I Move to a separate interface, like IInjectionFactoryInitializer?
    /// <summary>
    /// Initializes the injection factory. This method will be called once after creation
    /// of the factory.
    /// </summary>
    /// <remarks>
    /// Because the injection factory is specified in an attribute and we don't yet have
    /// an inversion of control container available to use, we rely on temporal coupling
    /// (i.e., the call to this method before the call to <see cref="Create"/>) as a means
    /// of performing necessary setup and configuration of the injection factory.
    /// </remarks>
    /// <param name="typeDiscoverer">
    /// The type discoverer to be used by the <see cref="IInjectionFactory"/>.
    /// </param>
    void Initialize(ITypeDiscoverer typeDiscoverer);

    // TODO: Should throw an exception that can be expected by any implementations.
    /// <summary>
    /// Method used to create the type being injected into a test fixture.
    /// </summary>
    /// <param name="type">The type to be created.</param>
    /// <returns>The created object; never null.</returns>
    /// <exception cref="Exception">
    /// Thrown when an error occurs creating the requested dependency.
    /// </exception>
    object Create(Type type);
  }
}