// Copyright (c) Kaleb Pederson Software LLC. All rights reserved.
// Licensed under the MIT license. See LICENSE file alongside the solution file for full license information.

using System;

namespace NUnit.Extension.DependencyInjection.Abstractions
{
  /// <summary>
  /// <para>
  /// Attribute whose presence within an assembly indicates that the
  /// assembly should be scanned when types are being discovered
  /// by potentially dangerous <see
  /// cref="NUnit.Extension.DependencyInjection.Abstractions.ITypeDiscoverer"/>s.
  /// </para>
  /// <para>
  /// An <see 
  /// cref="NUnit.Extension.DependencyInjection.Abstractions.ITypeDiscoverer"/>
  /// is potentially dangerous if there's a high risk that it will potentially
  /// pull in types that should not automatically be registered.
  /// </para>
  /// </summary>
  [AttributeUsage(AttributeTargets.Assembly)]
  public class NUnitAutoScanAssemblyAttribute : Attribute
  {
  }
}