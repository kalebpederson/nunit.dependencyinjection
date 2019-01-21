// Copyright (c) Kaleb Pederson Software LLC. All rights reserved.
// Licensed under the MIT license. See LICENSE file alongside the solution file for full license information.

using System;
using System.Reflection;

namespace NUnit.Extension.DependencyInjection
{
  /// <summary>
  /// Provides validation of type discoverer types.
  /// </summary>
  public static class TypeDiscovererTypeValidator
  {
    /// <summary>
    /// Validates that the <paramref name="discovererType"/> is valid.
    /// Or, in other words, that the following conditions are met:
    /// <list type="bullet">
    /// <item><paramref name="discovererType"/> is not null.</item>
    /// <item>Type implements the <see cref="ITypeDiscoverer"/> interface.</item>
    /// <item>Type has a public no-args constructor.</item>
    /// </list>
    /// </summary>
    /// <param name="discovererType">The type to be validated.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="discovererType"/> is null.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="discovererType"/> does not
    /// implement <see cref="ITypeDiscoverer"/>.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="discovererType"/> does not
    /// have a public no-args constructor.
    /// </exception>
    public static void AssertIsValidDiscovererType(Type discovererType)
    {
      AssertIsNotNull(discovererType);
      AssertImplementsProperInterface(discovererType);
      AssertHasPublicNoArgsConstructor(discovererType);
    }

    internal static void AssertIsNotNull(Type discovererType)
    {
      if (discovererType == null)
      {
        throw new ArgumentNullException(
          $"{nameof(discovererType)} specified as {nameof(ITypeDiscoverer)} on " +
          $"{nameof(NUnitTypeDiscovererAttribute)} cannot be null."
        );
      }
    }

    internal static void AssertHasPublicNoArgsConstructor(Type discovererType)
    {
      var ctorInfo = discovererType.GetConstructor(
        BindingFlags.Public | BindingFlags.Instance,
        null,
        CallingConventions.Standard,
        Type.EmptyTypes,
        null);
      if (ctorInfo == null)
      {
        throw new ArgumentOutOfRangeException(
          nameof(discovererType),
          $"{discovererType.FullName} specified as {nameof(ITypeDiscoverer)} on " +
          $"{nameof(NUnitTypeDiscovererAttribute)} must have a public no-args constructor."
        );
      }
    }

    internal static void AssertImplementsProperInterface(Type discovererType)
    {
      if (!typeof(ITypeDiscoverer).IsAssignableFrom(discovererType))
      {
        throw new ArgumentOutOfRangeException(
          $"{nameof(discovererType)} specified on {nameof(NUnitTypeDiscovererAttribute)} " +
          $"must be of type {nameof(ITypeDiscoverer)}."
        );
      }
    }
  }
}