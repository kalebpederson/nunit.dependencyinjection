// Copyright (c) Kaleb Pederson Software LLC. All rights reserved.
// Licensed under the MIT license. See LICENSE file alongside the solution file for full license information.

using System;

namespace NUnit.Extension.DependencyInjection
{
  /// <summary>
  /// Convenience type factory selector that returns the type provided at the
  /// time the instance of this class was created.
  /// </summary>
  public class StaticInjectionFactoryTypeSelector : IInjectionFactoryTypeSelector
  {
    private readonly Type _type;

    /// <summary>
    /// Creates an instance of the class which will always return
    /// <paramref name="type"/> as its <see cref="IInjectionFactory"/>.
    /// </summary>
    /// <remarks>
    /// This class is generally only used for writing unit tests and/or
    /// other dependency injection extensions.
    /// </remarks>
    /// <param name="type">
    /// The type of <see cref="IInjectionFactory"/>.
    /// </param>
    public StaticInjectionFactoryTypeSelector(Type type)
    {
      _type = type;
    }

    /// <inheritdoc />
    public Type GetInjectionType()
    {
      return _type;
    }
  }
}