// Copyright (c) Kaleb Pederson Software LLC. All rights reserved.
// Licensed under the MIT license. See LICENSE file alongside the solution file for full license information.

using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using NUnit.Extension.DependencyInjection.Abstractions;

namespace NUnit.Extension.DependencyInjection
{
  /// <summary>
  /// Attribute by which the <see cref="NUnit.Extension.DependencyInjection.Abstractions.IInjectionFactoryTypeSelector"/> is chosen. When this
  /// attribute is applied to the assembly all loaded assemblies for the current <see
  /// cref="AppDomain"/> are scanned for this assembly and the <b>first</b> discovered one
  /// is used to determine the injection factory to be used.
  /// </summary>
  /// <exception cref="InvalidOperationException">
  /// Thrown when no <see cref="NUnitTypeInjectionFactoryAttribute"/> is found among the
  /// loaded assemblies.
  /// </exception>
  /// <exception cref="ArgumentNullException">
  /// Thrown when the type returned by
  /// <see cref="NUnitTypeInjectionFactoryAttribute.InjectionFactoryType" />
  /// is null.
  /// </exception>
  /// <exception cref="ArgumentOutOfRangeException">
  /// Thrown when the type returned by
  /// <see cref="NUnitTypeInjectionFactoryAttribute.InjectionFactoryType" />
  /// does not implement <see cref="NUnit.Extension.DependencyInjection.Abstractions.IInjectionFactory"/> or does not have a public
  /// no-args constructor.
  /// </exception>
  public class AttributeBasedInjectionFactoryTypeSelector : IInjectionFactoryTypeSelector
  {
    /// <inheritdoc />
    public Type GetInjectionType()
    {
      return GetInjectionFactoryTypeFromAttribute();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Type GetInjectionFactoryTypeFromAttribute()
    {
      var injectionFactoryAttribute = AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(a => a.GetCustomAttributes(typeof(NUnitTypeInjectionFactoryAttribute), false))
        .OfType<NUnitTypeInjectionFactoryAttribute>()
        .FirstOrDefault();
      if (injectionFactoryAttribute == null)
      {
        throw new InvalidOperationException(
          $"{nameof(DependencyInjectingTestFixtureAttribute)} requires an injection plugin be loaded. Please ensure " +
          $"that one is present or create one using the {typeof(IInjectionFactory).FullName} interface " +
          $"and register it using the {typeof(NUnitTypeInjectionFactoryAttribute).FullName} attribute.");
      }
      InjectionFactoryTypeValidator.AssertIsValidFactoryType(injectionFactoryAttribute.InjectionFactoryType);
      Trace.TraceInformation(
        $"Found {nameof(NUnitTypeInjectionFactoryAttribute)} in assembly " +
        $"{injectionFactoryAttribute.GetType().Assembly.FullName}. Will use the {injectionFactoryAttribute.InjectionFactoryType} type " +
        "to create dependencies.");
      return injectionFactoryAttribute.InjectionFactoryType;
    }
  }
}