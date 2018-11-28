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
  public class UnityIocContainer : IIocContainer
  {
    private readonly Action<IUnityContainer> _initializer;
    private readonly Func<IUnityContainer> _containerFactory;

    [ExcludeFromCodeCoverage]
    public UnityIocContainer()
    {
      _containerFactory = () => Singleton<UnityContainer>.Instance;
      _initializer = FunctionalHelpers.CreateRunOnce<IUnityContainer>(
        // TODO: Need to use type discoverers instead of the hard coded value
        container => new ConventionMappingTypeDiscoverer().Discover(container)
        );
    }

    public UnityIocContainer(Action<IUnityContainer> initializer, Func<IUnityContainer> containerFactory)
    {
      _initializer = initializer;
      _containerFactory = containerFactory;
    }

    /// <inheritdoc />
    public object Create(Type type)
    {
      // NOTE: although the IUnityContainer is disposable, this should be
      // the global instance and it should not be disposed of at this point.
      var container = _containerFactory.Invoke();
      _initializer.Invoke(container);
      return container.Resolve(type);
    }
  }
}