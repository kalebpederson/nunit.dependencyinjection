// Copyright (c) Kaleb Pederson Software LLC. All rights reserved.
// Licensed under the MIT license. See LICENSE file alongside the solution file for full license information.

using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
        .Where(AssemblyCanBeLoaded)
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

    private static bool AssemblyCanBeLoaded(Assembly assembly)
    {
      try
      {
        return assembly.GetTypes().Any();
      }
      catch (ReflectionTypeLoadException ex)
      {
        // This isn't our fault, and there's nothing we can do with about it.
        // Erroring out and aborting all loading isn't going to help up so
        // it is only reasonable to skip these assemblies.
        Trace.TraceWarning($"Unable to load assembly {assembly.FullName}: {ex.Message}");
        return false;
      }
    }
  }
}