﻿// Copyright (c) Kaleb Pederson Software LLC. All rights reserved.
// Licensed under the MIT license. See LICENSE file alongside the solution file for full license information.

using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using NUnit.Extension.DependencyInjection.Abstractions;
using Unity;

namespace NUnit.Extension.DependencyInjection.Unity
{
  /// <summary>
  /// A type discoverer which does discovery of all <see cref="NUnit.Extension.DependencyInjection.Abstractions.IIocRegistrar"/>s
  /// available after which the discovered registrars are executed. Registrar
  /// types which are decorated with the <see
  /// cref="NUnit.Extension.DependencyInjection.Abstractions.NUnitExcludeFromAutoScanAttribute"/> are excluded from discovery and,
  /// therefore, will not be executed.
  /// </summary>
  public class IocRegistrarTypeDiscoverer : TypeDiscovererBase<IUnityContainer>
  {
    /// <inheritdoc />
    protected override void DiscoverInternal(IUnityContainer container)
    {
      var types = AppDomain.CurrentDomain.GetAssemblies()
        .Where(AssemblyCanBeLoaded)
        .SelectMany(a => a.GetTypes())
        .Where(IsAnIIocRegistrar)
        .Where(IsTypeIncludedInScanning)
        .ToList();
      foreach (var registrar in types)
      {
        ResolveAndRunRegistrar(container, registrar);
      }
    }

    private static bool IsAnIIocRegistrar(Type t)
    {
      return typeof(IIocRegistrar).IsAssignableFrom(t) && !t.IsAbstract;
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
        for (var i=0; i<ex.LoaderExceptions.Length; ++i)
        {
          Trace.TraceWarning($"  [{i}] => {ex.LoaderExceptions[i].Message}");
        }
        return false;
      }
    }
    
    private static bool IsTypeIncludedInScanning(Type t)
    {
      return !t.GetCustomAttributes(typeof(NUnitExcludeFromAutoScanAttribute), true).Any();
    }
  }
}