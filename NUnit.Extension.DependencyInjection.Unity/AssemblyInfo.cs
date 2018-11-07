using System.Runtime.CompilerServices;
using NUnit.Extension.DependencyInjection;
using NUnit.Extension.DependencyInjection.Unity;

[assembly: NUnitTypeInjectionFactory(typeof(UnityInjectionFactory))]
[assembly: InternalsVisibleTo("NUnit.Extension.DependencyInjection.Unity.Tests")]
