using NUnit.Extension.DependencyInjection;
using NUnit.Extension.DependencyInjection.Unity;
using NUnit.Framework;

[assembly: NUnitTypeInjectionFactory(typeof(UnityInjectionFactory))]
[assembly: NUnitTypeDiscoverer(typeof(ConventionMappingTypeDiscoverer))]
[assembly: NUnitAutoScanAssembly]

namespace NUnit.Extension.DependencyInjection.Unity.IntegrationTests
{
  public interface IDoStuff {}

  public class DoStuff : IDoStuff {}

  public interface IRequireDeps
  {
    IDoStuff DoStuff { get; }
  }

  public class RequireDeps : IRequireDeps
  {
    public RequireDeps(IDoStuff doStuff)
    {
      DoStuff = doStuff;
    }

    public IDoStuff DoStuff { get; }
  }


  [DependencyInjectingTestFixture]
  public class HappyPathTests
  {
    private readonly IRequireDeps _requireDeps;
    private readonly IDoStuff _doStuff;

    public HappyPathTests(IRequireDeps requireDeps, IDoStuff doStuff)
    {
      _requireDeps = requireDeps;
      _doStuff = doStuff;
    }

    [Test]
    public void Unity_Extension_can_inject_multiple_dependencies()
    {
      Assert.That(_requireDeps, Is.Not.Null);
      Assert.That(_doStuff, Is.Not.Null);
    }

    [Test]
    public void Unity_Extension_can_inject_a_dependency_that_has_dependencies()
    {
      Assert.That(_requireDeps, Is.Not.Null);
      Assert.That(_requireDeps.DoStuff, Is.Not.Null);
    }
  }
}