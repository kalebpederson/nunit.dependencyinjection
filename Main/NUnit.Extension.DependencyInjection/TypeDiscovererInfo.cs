// Copyright (c) Kaleb Pederson Software LLC. All rights reserved.
// Licensed under the MIT license. See LICENSE file alongside the solution file for full license information.

using System;

namespace NUnit.Extension.DependencyInjection
{
  /// <summary>
  /// Container for holding the 
  /// </summary>
  public class TypeDiscovererInfo
  {
    /// <summary>
    /// The type of <see cref="ITypeDiscoverer"/> being requested.
    /// </summary>
    public Type DiscovererType { get; set;}
    
    /// <summary>
    /// Arguments necessary for construction of the <see cref="ITypeDiscoverer"/>.
    /// </summary>
    public object[] DiscovererArguments { get; set;}
    
    /// <summary>
    /// The types of the arguments necessary for construction of the
    /// <see cref="ITypeDiscoverer"/>.
    /// </summary>
    public Type[] DiscovererArgumentTypes { get; set;}
  }
}