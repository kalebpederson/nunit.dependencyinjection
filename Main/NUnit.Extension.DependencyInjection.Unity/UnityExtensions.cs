// Copyright (c) Kaleb Pederson Software LLC. All rights reserved.
// Licensed under the MIT license. See LICENSE file alongside the solution file for full license information.

using System;
using NUnit.Extension.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Unity
{
  /// <summary>
  /// Extensions for <see cref="IUnityContainer"/>.
  /// </summary>
  public static class UnityExtensions
  {
    /// <summary>
    /// Registers the registrar with the IoC container.
    /// </summary>
    /// <param name="container">
    /// The IoC container with which the registrar will be registered.
    /// </param>
    /// <param name="registrar">
    /// The registrar to be registered.
    /// </param>
    /// <returns>The <paramref name="container"/>.</returns>
    public static IUnityContainer RegisterRegistrar(this IUnityContainer container, IIocRegistrar registrar)
    {
      if (registrar is null)
      {
        throw new ArgumentNullException(nameof(registrar));
      }
      
      registrar.Register(container);
      return container;
    }
  }
}