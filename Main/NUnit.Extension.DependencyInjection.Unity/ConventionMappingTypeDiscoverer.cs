// Copyright (c) Kaleb Pederson Software LLC. All rights reserved.
// Licensed under the MIT license. See LICENSE file alongside the solution file for full license information.

using System;
using System.Linq;
using System.Reflection;
using NUnit.Extension.DependencyInjection.Abstractions;
using Unity;
using Unity.RegistrationByConvention;

namespace NUnit.Extension.DependencyInjection.Unity
{
  /// <summary>
  /// <para>
  /// An <see cref="ITypeDiscoverer"/> whose implementation identifies all loaded
  /// assemblies marked with the <see cref="NUnitAutoScanAssemblyAttribute"/> and
  /// identifies concrete types with matching interfaces using Unity's <see
  /// cref="WithMappings.FromMatchingInterface"/> mechanism for registering types
  /// with the inversion of control container.
  /// </para>
  /// <para>
  /// Classes that are decorated with the <see cref="NUnitExcludeFromAutoScanAttribute"/>
  /// will be excluded from convention scanning.
  /// </para>
  /// <para>
  /// Note that this convention is potentially dangerous and, thus, it takes an opt-in approach
  /// used to reduce the potential damage done. It does so by limiting the assemblies scanned to
  /// those that are decorated with a <see cref="NUnitAutoScanAssemblyAttribute"/> attribute.
  /// </para>
  /// </summary>
  /// <remarks>
  /// Only assemblies loaded into the current application domain will be searched.
  /// If other assemblies are present on the filesystem, but have not been loaded,
  /// they will not be included in the search.
  /// </remarks>
  public class ConventionMappingTypeDiscoverer : TypeDiscovererBase<IUnityContainer>
  {
    /// <inheritdoc/>
    /// <summary>
    /// <para>
    /// An <see cref="NUnit.Extension.DependencyInjection.Abstractions.ITypeDiscoverer"/> whose implementation identifies all loaded
    /// assemblies marked with the <see cref="NUnitAutoScanAssemblyAttribute"/> and
    /// identifies concrete types with matching interfaces using Unity's <see
    /// cref="WithMappings.FromMatchingInterface"/> mechanism for registering types
    /// with the inversion of control container.
    /// </para>
    /// <para>
    /// Classes that are decorated with the <see cref="NUnit.Extension.DependencyInjection.Abstractions.NUnitExcludeFromAutoScanAttribute"/>
    /// will be excluded from convention scanning.
    /// </para>
    /// </summary>
    /// <param name="container">
    /// The container with which the dependencies will be registered.
    /// </param>
    protected override void DiscoverInternal(IUnityContainer container)
    {
      try
      {
        container.RegisterTypes(
          AppDomain.CurrentDomain.GetAssemblies()
            .Where(IsAssemblyAutoScanned)
            .SelectMany(x => x.GetTypes())
            .Where(x => !x.IsAbstract)
            .Where(IsTypeIncludedInScanning),
          WithMappings.FromMatchingInterface,
          WithName.Default,
          WithLifetime.Hierarchical
        );
      }
      catch (Exception ex)
      {
        throw new TypeDiscoveryException(GetType(), ex);
      }
    }

    private static bool IsTypeIncludedInScanning(Type t)
    {
      return !t.GetCustomAttributes(typeof(NUnitExcludeFromAutoScanAttribute), true).Any();
    }

    private static bool IsAssemblyAutoScanned(Assembly assembly)
    {
      return assembly.GetCustomAttributes(typeof(NUnitAutoScanAssemblyAttribute), true).Any();
    }
  }
}