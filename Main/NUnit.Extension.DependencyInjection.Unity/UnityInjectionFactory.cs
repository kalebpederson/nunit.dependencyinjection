using System;
using System.Diagnostics.CodeAnalysis;
using Unity;

namespace NUnit.Extension.DependencyInjection.Unity
{
  /// <summary>
  /// This class is usually referenced through the <see
  /// cref="NUnitTypeInjectionFactoryAttribute"/> as a means of specifying
  /// the concrete type that should be used to create the instances which
  /// are injected into the test classes.
  /// </summary>
  /// <example>
  /// [assembly: NUnitTypeInjectionFactory(typeof(UnityIocContainer))]
  /// </example>
  public class UnityInjectionFactory : IInjectionFactory
  {
    private readonly Lazy<IUnityContainer> _lazyContainer;

    [ExcludeFromCodeCoverage]
    public UnityInjectionFactory()
    {
      _lazyContainer = new Lazy<IUnityContainer>(() => Singleton<UnityContainer>.Instance, true);
    }

    public UnityInjectionFactory(Func<IUnityContainer> lazyContainer)
    {
      _lazyContainer = new Lazy<IUnityContainer>(lazyContainer, true);
    }

    /// <inheritdoc />
    public void Initialize(ITypeDiscoverer typeDiscoverer)
    {
      // FIXME ? : I don't like the temporal coupling of initializer methods
      
      // NOTE: although the IUnityContainer is disposable, this should be
      // the global instance and it should not be disposed of at this point.
      typeDiscoverer.Discover(_lazyContainer.Value);
    }

    /// <inheritdoc />
    public object Create(Type type)
    {
      return _lazyContainer.Value.Resolve(type);
    }
  }
}