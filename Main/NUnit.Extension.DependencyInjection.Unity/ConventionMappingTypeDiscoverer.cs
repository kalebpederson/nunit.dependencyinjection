// Copyright (c) Kaleb Pederson Software LLC. All rights reserved.
// Licensed under the MIT license. See LICENSE file alongside the solution file for full license information.

using System;
using System.Linq;
using Unity;
using Unity.RegistrationByConvention;

namespace NUnit.Extension.DependencyInjection.Unity
{
  /// <summary>
  /// An <see cref="ITypeDiscoverer"/> whose implementation identifies all loaded
  /// assemblies marked with the <see cref="NUnitAutoScanAssemblyAttribute"/> and
  /// identifies types with matching interfaces using Unity's <see
  /// cref="WithMappings.FromMatchingInterface"/> mechanism for registering types
  /// with the inversion of control container.
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
    /// Registers types from all assemblies marked with the <see
    /// cref="NUnitAutoScanAssemblyAttribute"/> and identifies
    /// types with matching interfaces using Unity's <see
    /// cref="WithMappings.FromMatchingInterface"/> mechanism for registering types
    /// with the inversion of control container.
    /// </summary>
    /// <param name="container">
    /// The container with which the dependencies will
    /// be registered.
    /// </param>
    protected override void DiscoverInternal(IUnityContainer container)
    {
      try
      {
        container.RegisterTypes(
          AppDomain.CurrentDomain.GetAssemblies()
            // .Where(a => !a.GetCustomAttributes(typeof(NUnitExcludeFromAutoScanAttribute), true).Any())
            .Where(a => a.GetCustomAttributes(typeof(NUnitAutoScanAssemblyAttribute), true).Any())
            .SelectMany(x => x.GetTypes()),
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
  }
}