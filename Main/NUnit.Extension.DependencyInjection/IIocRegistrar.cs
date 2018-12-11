namespace NUnit.Extension.DependencyInjection
{
  /// <summary>
  /// Interface by which registration of dependencies with the inversion of
  /// control container takes place when performing explicit registrations.
  /// </summary>
  public interface IIocRegistrar
  {
    /// <summary>
    /// The method called to register dependencies with the <paramref
    /// name="container"/> when using a registrar based registration process.
    /// </summary>
    /// <param name="container">
    /// The container into which dependencies will be registered.
    /// </param>
    void Register(object container);
  }
}