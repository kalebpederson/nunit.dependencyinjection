// Copyright (c) Kaleb Pederson Software LLC. All rights reserved.
// Licensed under the MIT license. See LICENSE file alongside the solution file for full license information.

using System;
using Unity;

namespace NUnit.Extension.DependencyInjection.Unity
{
  /// <summary>
  /// A type discoverer which does discovery based on the set of discovered type
  /// registers but otherwise does no scanning of loaded assemblies.
  /// </summary>
  public class ManualRegistrarTypeDiscoverer : TypeDiscovererBase<IUnityContainer>
  {
    private readonly Lazy<Type> _lazyRegistrarType;
    
    /// <summary>
    /// Creates an instance of the type discoverer.
    /// </summary>
    /// <param name="registrarType">The registrar to be used to register the types.</param>
    public ManualRegistrarTypeDiscoverer(Type registrarType)
    {
      _lazyRegistrarType = new Lazy<Type>(() => ValidatedRegistrarType(registrarType), true);
    }

    internal Type ValidatedRegistrarType(Type registrarType)
    { 
      if (typeof(IIocRegistrar).IsAssignableFrom(registrarType) && !registrarType.IsAbstract)
      {
        return registrarType;
      }
      throw new ArgumentOutOfRangeException(
        $"{nameof(registrarType)} must be an instantiable subclass of {nameof(IIocRegistrar)}"
        );
    }

    /// <inheritdoc />
    protected override void DiscoverInternal(IUnityContainer container)
    {
      try
      {
        var registrar = ResolveRegistrarInstance(container, _lazyRegistrarType.Value);
        container.RegisterRegistrar(registrar);
      }
      catch (Exception ex)
      {
        throw new TypeDiscoveryException(GetType(), ex);
      }
    }

    private IIocRegistrar ResolveRegistrarInstance(IUnityContainer container, Type registrarType)
    {
      return (IIocRegistrar)container.Resolve(registrarType);
    }
  }
}
