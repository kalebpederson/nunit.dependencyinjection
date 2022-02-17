// Copyright (c) Kaleb Pederson Software LLC. All rights reserved.
// Licensed under the MIT license. See LICENSE file alongside the solution file for full license information.

using System;

namespace NUnit.Extension.DependencyInjection.Abstractions
{
  /// <summary>
  /// Attribute whose presence on a type indicates that the type should
  /// not be included in convention based scanning performed by any
  /// <see cref="NUnit.Extension.DependencyInjection.Abstractions.ITypeDiscoverer"/>s in use in the system.
  /// </summary>
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
  public class NUnitExcludeFromAutoScanAttribute : Attribute
  {
  }
}