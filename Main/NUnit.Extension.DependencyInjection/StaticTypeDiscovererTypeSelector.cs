// Copyright (c) Kaleb Pederson Software LLC. All rights reserved.
// Licensed under the MIT license. See LICENSE file alongside the solution file for full license information.

using System;
using NUnit.Extension.DependencyInjection.Abstractions;

namespace NUnit.Extension.DependencyInjection
{
  /// <summary>
  /// <see cref="NUnit.Extension.DependencyInjection.Abstractions.ITypeDiscovererTypeSelector"/> that will always return the
  /// type provided at the time of construction of this type selector.
  /// </summary>
  public class StaticTypeDiscovererTypeSelector : ITypeDiscovererTypeSelector
  {
    private readonly Type _type;

    /// <summary>
    /// Creates an instance of the type selector.
    /// </summary>
    /// <param name="type">The <see cref="NUnit.Extension.DependencyInjection.Abstractions.ITypeDiscoverer"/> to be used.</param>
    public StaticTypeDiscovererTypeSelector(Type type)
    {
      _type = type;
    }

    /// <inheritdoc />
    public Type GetTypeDiscovererType()
    {
      return _type;
    }

    /// <inheritdoc />
    public object[] GetTypeDiscovererArguments()
    {
      // FIXME: Do I need to do anything more?
      return new object[] { };
    }
  }
}