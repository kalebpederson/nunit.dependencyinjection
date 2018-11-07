= Introduction =

Outside of pure unit tests it sometimes becomes desirable to do dependency
injection in automated tests. By their nature these aren't the typical unit
tests as they have dependencies. These could be, for example, acceptance
tests against a standalone application. The dependencies to be injected, in
this case, might be things to provide information about the environment to
hit, such as hostnames and database connection strings, or may be client
SDKs to the system under test. This framework is intended to support these
scenarios.

= Warning =
Good unit tests shouldn't need dependency injection. If you need dependency
injection in unit tests then consider whether the design of the system needs
to change. From here on out it is assumed that the purpose for dependency
injection within tests has been validated and is appropriate.

= API Stability =
In accordance with semantic versioning, all versions prior to 1.0 are assumed
pre-releases and are **NOT** subject to API/ABI compatibility.

= Usage = 

The below is subject to change at our whims until version 1.0:

```
// Import the following NuGet packages: 
// * NUnit.Extension.DependencyInjection
// * NUnit.Extension.DependencyInjection.Unity

// Add once to the test assembly
[assembly: NUnit.Extension.DependencyInjection.Unity.ScanInContainer]

// Loaded assemblies are scanned for interfaces and corresponding concrete
// definitions. For example:
public interface IDependency1 {}
public interface IDependency2 {}

public class Dependency1 : IDependency1 { }

// Instead of using the [TestFixture] attribute when declaring a test class
// do the following:
[NUnit.Extension.DependencyInjection.DependencyInjectingTestFixture]
public class MyTests
{
  private readonly IDependency1 _dependency1;
  private readonly IDependency2 _dependency2;

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


