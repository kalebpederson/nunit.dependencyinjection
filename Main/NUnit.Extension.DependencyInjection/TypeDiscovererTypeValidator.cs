// Copyright (c) Kaleb Pederson Software LLC. All rights reserved.
// Licensed under the MIT license. See LICENSE file alongside the solution file for full license information.

using System;
using System.Linq;
using System.Reflection;
using NUnit.Extension.DependencyInjection.Abstractions;

namespace NUnit.Extension.DependencyInjection
{
  /// <summary>
  /// Provides validation of type discoverer types.
  /// </summary>
  public static class TypeDiscovererTypeValidator
  {
    /// <summary>
    /// Validates that the information provided around the type discoverer
    /// is valid. For example, ensures that the provided type information
    /// refers to an <see cref="NUnit.Extension.DependencyInjection.Abstractions.ITypeDiscoverer" /> that can be constructed.
    /// Or, in other words, that the following conditions are met:
    /// <list type="bullet">
    /// <item>The type discoverer type information is not null.</item>
    /// <item>Type discoverer type implements the <see cref="NUnit.Extension.DependencyInjection.Abstractions.ITypeDiscoverer"/>
    /// interface.</item>
    /// <item>Type has a public no-args constructor or a constructor which
    /// matches the provided argument type information.</item>
    /// </list>
    /// </summary>
    /// <param name="info">
    /// The details surrounding the 
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="info"/> has a <see
    /// cref="TypeDiscovererInfo.DiscovererType" /> that is null.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="info"/> has a <see
    /// cref="TypeDiscovererInfo.DiscovererType" /> does not
    /// implement <see cref="NUnit.Extension.DependencyInjection.Abstractions.ITypeDiscoverer"/>.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="info"/> has a <see
    /// cref="TypeDiscovererInfo.DiscovererType" /> that does not
    /// have a constructor which matches the provided <see
    /// cref="TypeDiscovererInfo.DiscovererArgumentTypes" />
    /// </exception>
    public static void AssertIsValidDiscovererType(TypeDiscovererInfo info)
    {
      if (info is null)
      {
        throw new ArgumentNullException(nameof(info), $"{nameof(info)} must be non-null.");
      }
      AssertIsNotNull(info.DiscovererType);
      AssertImplementsProperInterface(info.DiscovererType);
      AssertHasArgumentsThatMatchConstructorArguments(info);
    }

    private static void AssertHasArgumentsThatMatchConstructorArguments(TypeDiscovererInfo info)
    {
      var discovererType = info.DiscovererType;
      var argumentTypes = info.DiscovererArgumentTypes;
      var argumentTypeNames = argumentTypes.Select(x => x.FullName);
      var ctorInfo = info.DiscovererType.GetConstructor(
        BindingFlags.Public | BindingFlags.Instance,
        null,
        CallingConventions.Standard,
        argumentTypes,
        null);
      if (ctorInfo == null)
      {
        throw new ArgumentOutOfRangeException(
          nameof(info),
          $"{discovererType.FullName} specified as {nameof(ITypeDiscoverer)} on " +
          $"{nameof(NUnitTypeDiscovererAttribute)} must have a constructor which corresponds " +
          $"to the argument types which were provided: {string.Join(", ", argumentTypeNames)}."
        );
      }
    }

    private static void AssertIsNotNull(Type discovererType)
    {
      if (discovererType == null)
      {
        throw new ArgumentNullException(
          $"{nameof(discovererType)} specified as {nameof(ITypeDiscoverer)} on " +
          $"{nameof(NUnitTypeDiscovererAttribute)} cannot be null."
        );
      }
    }

    private static void AssertImplementsProperInterface(Type discovererType)
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