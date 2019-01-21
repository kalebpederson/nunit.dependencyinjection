// Copyright (c) Kaleb Pederson Software LLC. All rights reserved.
// Licensed under the MIT license. See LICENSE file alongside the solution file for full license information.

using System.Runtime.CompilerServices;
using NUnit.Extension.DependencyInjection;
using NUnit.Extension.DependencyInjection.Unity;

[assembly: NUnitTypeInjectionFactory(typeof(UnityInjectionFactory))]
[assembly: InternalsVisibleTo("NUnit.Extension.DependencyInjection.Unity.Tests")]
