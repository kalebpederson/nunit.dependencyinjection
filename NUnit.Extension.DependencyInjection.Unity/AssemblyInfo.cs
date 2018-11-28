using System.Runtime.CompilerServices;
using NUnit.Extension.DependencyInjection;
using NUnit.Extension.DependencyInjection.Unity;

[assembly: NUnitTypeInjectionFactory(typeof(UnityIocContainer))]
[assembly: InternalsVisibleTo("NUnit.Extension.DependencyInjection.Unity.Tests")]
