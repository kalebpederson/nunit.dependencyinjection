using System;
using System.Diagnostics.CodeAnalysis;
using Autofac;

namespace NUnit.Extension.DependencyInjection.Autofac
{
  /// <summary>
  /// This class is an <see cref="IInjectionFactory"/> referenced through the
  /// <see cref="NUnitTypeInjectionFactoryAttribute"/> as a means of specifying
  /// the concrete type that is used to create the instances which are 
  /// injected into test fixtures decorated with the <see
  /// cref="DependencyInjectingTestFixtureAttribute"/>.
  /// </summary>
  /// <example>
  /// [assembly: NUnitTypeInjectionFactory(typeof(AutofacIocContainer))]
  /// </example>
  public class AutofacInjectionFactory : IInjectionFactory
  {
    private readonly Lazy<IContainer> _lazyContainer;

    /// <summary>
    /// Creates an instance of the <see cref="AutofacInjectionFactory"/> configured
    /// to use a singleton <see cref="IContainer"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public AutofacInjectionFactory()
    {
      _lazyContainer = new Lazy<IContainer>(() => Singleton<ContainerBuilder>.Instance.Build(), true);
    }

    /// <summary>
    /// <para>
    /// Creates an instance of the <see cref="AutofacInjectionFactory"/> configured
    /// to use the <paramref name="lazyContainer"/> function to create the container
    /// that will be used to resolve the parameters to the test fixtures.
    /// </para>
    /// <para>
    /// The <paramref name="lazyContainer"/> instance will be called once to create
    /// the container. All subsequent calls to the injection factory will use the
    /// previously created instance of the container.
    /// </para>
    /// </summary>
    /// <param name="lazyContainer">
    /// The factory function used to create the container.
    /// </param>
    public AutofacInjectionFactory(Func<ContainerBuilder> lazyContainer)
    {
      _lazyContainer = new Lazy<IContainer>(
        () => lazyContainer.Invoke().Build(), true);
    }

    /// <inheritdoc />
    public void Initialize(ITypeDiscoverer typeDiscoverer)
    {
      typeDiscoverer.Discover(_lazyContainer.Value);
    }

    /// <inheritdoc />
    public object Create(Type type)
    {
      return _lazyContainer.Value.Resolve(type);
    }
  }
}