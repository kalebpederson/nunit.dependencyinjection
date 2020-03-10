// Copyright (c) Kaleb Pederson Software LLC. All rights reserved.
// Licensed under the MIT license. See LICENSE file alongside the solution file for full license information.

using System;

namespace NUnit.Extension.DependencyInjection
{
  /// <summary>
  /// Attribute whose presence on a type indicates that the type should
  /// not be included in automatic registration by any
  /// <see cref="ITypeDiscoverer"/>s in use in the system.
  /// </summary>
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
  public class NUnitExcludeFromAutoScanAttribute : Attribute
  {
  }
}