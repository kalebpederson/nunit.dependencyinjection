// Copyright (c) Kaleb Pederson Software LLC. All rights reserved.
// Licensed under the MIT license. See LICENSE file alongside the solution file for full license information.

using System;
using System.Diagnostics;
using System.Linq;

namespace NUnit.Extension.DependencyInjection
{
  /// <summary>
  /// Discovers the <see cref="ITypeDiscoverer"/> that will be used to identify and
  /// register types with the inversion of control container. It does so based on
  /// the presence of the <see cref="NUnitTypeDiscovererAttribute"/> and its
  /// corresponding constructor parameter.
  /// </summary>
  public class AttributeBasedTypeDiscovererTypeSelector : ITypeDiscovererTypeSelector
  {
    /// <inheritdoc />
    public Type GetTypeDiscovererType()
    {
      var typeDiscovererAttribute = GetTypeDiscovererAttributeOrThrow();
      Trace.TraceInformation(
        $"Found {nameof(NUnitTypeDiscovererAttribute)} in assembly " +
        $"{typeDiscovererAttribute.GetType().Assembly.FullName}. Will use the type discoverer of type " +
        $"{typeDiscovererAttribute.TypeDiscovererInfo.DiscovererType.FullName}.");
      return typeDiscovererAttribute.TypeDiscovererInfo.DiscovererType;
    }
    
    /// <inheritdoc />
    public object[] GetTypeDiscovererArguments()
    {
      var typeDiscovererAttribute = GetTypeDiscovererAttributeOrThrow();
      return typeDiscovererAttribute.TypeDiscovererInfo.DiscovererArguments;
    }

    private static NUnitTypeDiscovererAttribute GetTypeDiscovererAttributeOrThrow()
    {
      var typeDiscovererAttribute = AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(a => a.GetCustomAttributes(typeof(NUnitTypeDiscovererAttribute), false))
        .OfType<NUnitTypeDiscovererAttribute>()
        .FirstOrDefault();
      if (typeDiscovererAttribute == null)
      {
        throw new InvalidOperationException(
          $"The {typeof(ITypeDiscoverer).FullName} registered requires the presence of at least one type " +
          $"discoverer but none were found.");
      }
      return typeDiscovererAttribute;
    }
  }
}