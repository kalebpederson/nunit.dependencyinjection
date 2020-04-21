// Copyright (c) Kaleb Pederson Software LLC. All rights reserved.
// Licensed under the MIT license. See LICENSE file alongside the solution file for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Extension.DependencyInjection.Abstractions;
using Unity;

namespace NUnit.Extension.DependencyInjection.Unity
{
  /// <summary>
  /// This class is an <see cref="NUnit.Extension.DependencyInjection.Abstractions.IInjectionFactory"/> referenced through the
  /// <see cref="NUnitTypeInjectionFactoryAttribute"/> as a means of specifying
  /// the concrete type that is used to create the instances which are
  /// injected into test fixtures decorated with the <see
  /// cref="DependencyInjectingTestFixtureAttribute"/>.
  /// </summary>
  /// <example>
  /// [assembly: NUnitTypeInjectionFactory(typeof(UnityIocContainer))]
  /// </example>
  public class UnityInjectionFactory : IInjectionFactory
  {
    private readonly Lazy<IUnityContainer> _lazyContainer;

    /// <summary>
    /// Creates an instance of the <see cref="UnityInjectionFactory"/> configured
    /// to use a singleton <see cref="UnityContainer"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public UnityInjectionFactory()
    {
      _lazyContainer = new Lazy<IUnityContainer>(() => Singleton<UnityContainer>.Instance, true);
    }

    /// <summary>
    /// <para>
    /// Creates an instance of the <see cref="UnityInjectionFactory"/> configured
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
    public UnityInjectionFactory(Func<IUnityContainer> lazyContainer)
    {
      _lazyContainer = new Lazy<IUnityContainer>(lazyContainer, true);
    }

    /// <inheritdoc />
    public void Initialize(ITypeDiscoverer typeDiscoverer)
    {
      if (typeDiscoverer is null)
      {
        throw new ArgumentNullException(
          nameof(typeDiscoverer),
          $"{nameof(typeDiscoverer)} passed to {GetType().FullName} was null.");
      }
      /* NOTE: although the IUnityContainer is disposable, this should be
         the global instance and it should not be disposed of at this point. */
      typeDiscoverer.Discover(_lazyContainer.Value);
    }

    /// <inheritdoc />
    public object Create(Type type)
    {
      return _lazyContainer.Value.Resolve(type);
    }
  }
}