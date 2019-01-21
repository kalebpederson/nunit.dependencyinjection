// Copyright (c) Kaleb Pederson Software LLC. All rights reserved.
// Licensed under the MIT license. See LICENSE file alongside the solution file for full license information.

using System;
using System.Linq;
using Unity;

namespace NUnit.Extension.DependencyInjection.Unity
{
  /// <summary>
  /// A type discoverer which does discovery based on the set of discovered type
  /// registers but otherwise does no scanning of loaded assemblies.
  /// </summary>
  public class IocRegistrarTypeDiscoverer : TypeDiscovererBase<IUnityContainer>
  {
    /// <inheritdoc />
    protected override void DiscoverInternal(IUnityContainer container)
    {
      var types = AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(a => a.GetTypes())
        .Where(t =>
          typeof(IIocRegistrar).IsAssignableFrom(t)
          && !t.IsAbstract)
        .ToList();
      foreach (var registrar in types)
      {
        ResolveAndRunRegistrar(container, registrar);
      }
    }

    private static void ResolveAndRunRegistrar(IUnityContainer container, Type registrar)
    {
      try
      {
        var registrarInstance = (IIocRegistrar) container.Resolve(registrar);
        registrarInstance.Register(container);
      }
      catch (Exception ex)
      {
        throw new TypeDiscoveryException(registrar, ex);
      }
    }
  }
}