// Copyright (c) Kaleb Pederson Software LLC. All rights reserved.
// Licensed under the MIT license. See LICENSE file alongside the solution file for full license information.

using System;

namespace NUnit.Extension.DependencyInjection
{
  /// <summary>
  /// Attribute that identifies the type of <see cref="ITypeDiscoverer"/>
  /// that should be used to discover the types that should be registered
  /// with the inversion of control container.
  /// </summary>
  [AttributeUsage(AttributeTargets.Assembly)]
  public class NUnitTypeDiscovererAttribute : Attribute
  {
    /// <summary>
    /// Container for the information necessary to create the
    /// type discoverer.
    /// </summary>
    public TypeDiscovererInfo TypeDiscovererInfo {get;}
 
    /// <summary>
    /// Creates an instance of the attribute specifying that the
    /// <see cref="ITypeDiscoverer"/> should be of type
    /// <paramref name="typeDiscovererType"/>.
    /// </summary>
    /// <param name="typeDiscovererType">
    /// The type of the <see cref="ITypeDiscoverer"/> to be used.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="typeDiscovererType"/> is null.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="typeDiscovererType"/> does not
    /// implement <see cref="ITypeDiscoverer"/>.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="typeDiscovererType"/> does not
    /// have a public no-args constructor.
    /// </exception>
    public NUnitTypeDiscovererAttribute(Type typeDiscovererType)
    {
      TypeDiscovererInfo = new TypeDiscovererInfo
      {
        DiscovererType = typeDiscovererType,
        DiscovererArgumentTypes = new Type[] {},
        DiscovererArguments = new object[] {}
      };
      TypeDiscovererTypeValidator.AssertIsValidDiscovererType(TypeDiscovererInfo);
    }

    /// <summary>
    /// Creates an instance of the attribute specifying that the
    /// <see cref="ITypeDiscoverer"/> should be of type
    /// <paramref name="typeDiscovererType"/>.
    /// </summary>
    /// <param name="typeDiscovererType">
    /// The type of the <see cref="ITypeDiscoverer"/> to be used.
    /// </param>
    /// <param name="typeDiscovererArgumentTypes"></param>
    /// <param name="typeDiscovererArguments">
    /// Parameters necessary for construction of the given type discoverer.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="typeDiscovererType"/> is null.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="typeDiscovererType"/> does not
    /// implement <see cref="ITypeDiscoverer"/>.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="typeDiscovererType"/> does not
    /// have a public no-args constructor.
    /// </exception>
    public NUnitTypeDiscovererAttribute(
      Type typeDiscovererType,
      Type[] typeDiscovererArgumentTypes,
      object[] typeDiscovererArguments)
    {
      TypeDiscovererInfo = new TypeDiscovererInfo
      {
        DiscovererType = typeDiscovererType,
        DiscovererArgumentTypes = typeDiscovererArgumentTypes,
        DiscovererArguments = typeDiscovererArguments
      };
      TypeDiscovererTypeValidator.AssertIsValidDiscovererType(TypeDiscovererInfo);
    }
  }
}