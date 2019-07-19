// Copyright (c) Kaleb Pederson Software LLC. All rights reserved.
// Licensed under the MIT license. See LICENSE file alongside the solution file for full license information.

using System;

namespace NUnit.Extension.DependencyInjection
{
  /// <summary>
  /// Attribute that identifies the type of <see cref="IInjectionFactory"/>
  /// that will be used to create and inject instances into the test fixtures.
  /// This will usually correspond to one of the factory types provided by a
  /// NUnit.Extension.DependencyInjection.* NuGet package, such as the
  /// <c>UnityInjectionFactory</c> provided by the
  /// NUnit.Extension.DependencyInjection.Unity package.
  /// </summary>
  [AttributeUsage(AttributeTargets.Assembly)]
  public class NUnitTypeInjectionFactoryAttribute : Attribute
  {
    /// <summary>
    /// The type of the configured <see cref="IInjectionFactory"/>.
    /// </summary>
    public Type InjectionFactoryType { get; }

    /// <summary>
    /// Creates an instance of the constructor declaring that the
    /// <see cref="IInjectionFactory"/> will be of type
    /// <paramref name="factoryType"/>.
    /// </summary>
    /// <param name="factoryType">The type of the factory to use.</param>
    public NUnitTypeInjectionFactoryAttribute(Type factoryType)
    {
      InjectionFactoryTypeValidator.AssertIsValidFactoryType(factoryType);

      InjectionFactoryType = factoryType;
    }
  }
}