# Introduction 

Outside of pure unit tests it sometimes becomes desirable to do dependency
injection in automated tests. By their nature these aren't the typical unit
tests as they have dependencies. These could be, for example, acceptance
tests against a standalone application. The dependencies to be injected, in
this case, might be things to provide information about the environment to
hit, such as hostnames and database connection strings, or may be client
SDKs to the system under test. This framework is intended to support these
scenarios.

# Warning
Good **unit tests** shouldn't need to inject dependencies into the test fixture.
If you need dependency injection into a test fixture in unit tests then
consider whether the design of the system could be changed in such a way that
the system under test can be easily unit tested without that need.

From here on out it is assumed that the purpose for dependency injection
within tests has been validated and is appropriate.

# API Stability
In accordance with semantic versioning, all versions prior to 1.0 are assumed
pre-releases and are **NOT** subject to API/ABI compatibility.

# Usage

The below is **subject to change at our whims until version 1.0**. For a
working example see the different test projects in the Validation folder.

Two different mechanisms are currently provided by which dependencies are
discovered, though custom processes can be added as well:

1. Explicit registration of dependencies via the `IIocRegistrar`
1. Convention based assembly scanning and registration of types

These approaches are described below.

## Discovery/Scanning of all `IIocRegistrar`s

When many different assemblies are present it can become difficult to
automatically register all interfaces with their corresponding concrete types.
Or, perhaps we prefer that everything be manually registered to make the
process and choice very explicit. The example below demonstrates that process:

```csharp
// First import the following NuGet packages: 
// * NUnit.Extension.DependencyInjection
// * NUnit.Extension.DependencyInjection.Unity

using NUnit.Extension.DependencyInjection;
using NUnit.Extension.DependencyInjection.Unity;

// tell the extension that we will be using the Microsoft Unity Injection
// factory
[assembly: NUnitTypeInjectionFactory(typeof(UnityInjectionFactory))]

// If we want to manually register the different types we need to create
// one or more implementations of IIocRegistrar that register with the
// container and then use the IocRegistrarTypeDiscoverer.
[assembly: NUnitTypeDiscoverer(typeof(IocRegistrarTypeDiscoverer))]

// The registrar above will scan for implementations of IIocRegistrar,
// which the RegistrarBase class implements, and then execute each
// discovered registrations:
public class MyRegistrar : RegistrarBase<IUnityContainer>
{
  protected override void RegisterInternal(IUnityContainer container)
  {
    container.RegisterType<IDependency1, Dependency1>();
    container.RegisterType<IDependency2, Dependency2>();
  }
}

// Loaded assemblies are scanned for interfaces and corresponding concrete
// definitions. For example:
public interface IDependency1 {}
public interface IDependency2 {}

public class Dependency1 : IDependency1 { }

// Instead of using the [TestFixture] attribute when declaring a test class
// you'll need to decorate the class with a DependencyInjectingTestFixture
// attribute:
[NUnit.Extension.DependencyInjection.DependencyInjectingTestFixture]
public class MyTests
{
  private readonly IDependency1 _dependency1;
  private readonly IDependency2 _dependency2;

  // once everything is properly configured, dependencies can be injected
  // directly into the constructor
  public MyTests(IDependency1 dependency1, IDependency2 dependency2)
  {
    _dependency1 = dependency1;
    _dependency2 = dependency2;
  }

  [Test]
  public void Test_something_using_IDependency1()
  {
    Assert.That(_dependency1, Is.Not.Null);
    Assert.That(_dependency1, Is.InstanceOf<Dependency1>());
  }
}
```

## Convention Based Type Discovery

Sometimes it's convenient if the inversion of control container picks up all
of the interfaces and their mapping registrations automatically. For example,
given an interface `IDateTimeProvider` and a corresponding implementation
`DateTimeProvider`, we might want to register the interface `IDateTimeProvider`
as corresponding to the concrete implementation of `DateTimeProvider`.
This example demonstrates that process:

```csharp
// First import the following NuGet packages: 
// * NUnit.Extension.DependencyInjection
// * NUnit.Extension.DependencyInjection.Unity

using NUnit.Extension.DependencyInjection;
using NUnit.Extension.DependencyInjection.Unity;

// tell the extension that we will be using the Microsoft Unity Injection
// factory
[assembly: NUnitTypeInjectionFactory(typeof(UnityInjectionFactory))]

// If we wanted to automatically register all interfaces with their concrete
// implementations for which the NUnitAutoScanAssembly attribute is present on
// the assembly is present we could do the following:
[assembly: NUnitTypeDiscoverer(typeof(ConventionMappingTypeDiscoverer))]

// When using the ConventionMappingTypeDiscoverer it can become problematic
// to scan lots of framework assemblies and types and pre-register them, so
// assemblies to be scanned **MUST** be decorated with the NUnitAutoScanAssembly
// attribute.
[assembly: NUnit.Extension.DependencyInjection.NUnitAutoScanAssembly]

// Loaded assemblies are scanned for interfaces and corresponding concrete
// definitions. For example:
public interface IDependency1 {}
public interface IDependency2 {}

public class Dependency1 : IDependency1 { }

// Instead of using the [TestFixture] attribute when declaring a test class
// you'll need to decorate the class with a DependencyInjectingTestFixture
// attribute:
[NUnit.Extension.DependencyInjection.DependencyInjectingTestFixture]
public class MyTests
{
  private readonly IDependency1 _dependency1;
  private readonly IDependency2 _dependency2;

  // once everything is properly configured, dependencies can be injected
  // directly into the constructor
  public MyTests(IDependency1 dependency1, IDependency2 dependency2)
  {
    _dependency1 = dependency1;
    _dependency2 = dependency2;
  }

  [Test]
  public void Test_something_using_IDependency1()
  {
    Assert.That(_dependency1, Is.Not.Null);
    Assert.That(_dependency1, Is.InstanceOf<Dependency1>());
  }
}
```

## Manual `IIocRegistrar` Selection

When conflicting `IIocRegistrars` are present it's not possible to use assembly
scanning. In this case, it's helpful to create a single `IIocRegistrar` that is
manually defined and which will perform all necessary registrations.


```csharp
// First import the following NuGet packages: 
// * NUnit.Extension.DependencyInjection
// * NUnit.Extension.DependencyInjection.Unity

using NUnit.Extension.DependencyInjection;
using NUnit.Extension.DependencyInjection.Unity;

// tell the extension that we will be using the Microsoft Unity Injection
// factory
[assembly: NUnitTypeInjectionFactory(typeof(UnityInjectionFactory))]

// If we wanted to automatically register all interfaces with their concrete
// implementations for which the NUnitAutoScanAssembly attribute is present on
// the assembly is present we could do the following:
[assembly: NUnitTypeDiscoverer(
  typeof(ManualRegistrarTypeDiscoverer<BundlingRegistrar>))]

public class BundlingRegistrar : RegistrarBase<IUnityContainer>
{
  /// <inheritdoc />
  protected override void RegisterInternal(IUnityContainer container)
  {
    container.RegisterType<IDependency1, Dependency1>();
    container.RegisterType<IDependency2, Dependency2>();
  }
}

// Loaded assemblies are scanned for interfaces and corresponding concrete
// definitions. For example:
public interface IDependency1 {}
public interface IDependency2 {}

public class Dependency1 : IDependency1 { }

// Instead of using the [TestFixture] attribute when declaring a test class
// you'll need to decorate the class with a DependencyInjectingTestFixture
// attribute:
[NUnit.Extension.DependencyInjection.DependencyInjectingTestFixture]
public class MyTests
{
  private readonly IDependency1 _dependency1;
  private readonly IDependency2 _dependency2;

  // once everything is properly configured, dependencies can be injected
  // directly into the constructor
  public MyTests(IDependency1 dependency1, IDependency2 dependency2)
  {
    _dependency1 = dependency1;
    _dependency2 = dependency2;
  }

  [Test]
  public void Test_something_using_IDependency1()
  {
    Assert.That(_dependency1, Is.Not.Null);
    Assert.That(_dependency1, Is.InstanceOf<Dependency1>());
  }
}
```
# Contributing

Please submit a PR. If the change is major it may be worth filing an issue first
even if only it only serves as a mechanism for discussion to ensure that the
work your doing aligns with the goals of this project.

For more information on the project structure and validation process please see
the [Development_Readme.md](Development_Readme.md) document.

# Troubleshooting

Although we have put a lot of effort into ensuring that the error messages are
descriptive and self-explanatory, unfortunately many of the test runners hide
error messages that occur while setting up the test. Suggested steps:

1. Verify that all the steps required to setup dependency injection for the
   given type discoverer have been performed.

1. If possible, run the test using `dotnet test` or the native NUnit test
   runner as they provide better error information, such as the full exception
   and stack trace.

# TODO

1. **Support for other containers** - Autofac and other containers that are more 
popular should be supported.