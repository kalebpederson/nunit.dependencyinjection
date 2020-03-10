using System;
using ManualRegistrarTypeDiscovererTests;
using NUnit.Extension.DependencyInjection;
using NUnit.Extension.DependencyInjection.Unity;
using NUnit.Framework;
using Unity;

[assembly: NUnitTypeInjectionFactory(typeof(UnityInjectionFactory))]
[assembly: NUnitTypeDiscoverer(
  typeof(ManualRegistrarTypeDiscoverer),
  new [] {typeof(Type)},
  new object[] {typeof(BundlingRegistrar)})]

namespace ManualRegistrarTypeDiscovererTests
{
  public interface IDoIt {}

  public class DoIt : IDoIt {}

  public interface IRequireDoIt
  {
    IDoIt DoIt { get; }
  }

  public class RequireDoIt : IRequireDoIt
  {
    public RequireDoIt(IDoIt doIt)
    {
      DoIt = doIt;
    }

    public IDoIt DoIt { get; }
  }

  internal class DoItRegistrar : RegistrarBase<IUnityContainer>
  {
    /// <inheritdoc />
    protected override void RegisterInternal(IUnityContainer container)
    {
      container.RegisterType<IDoIt, DoIt>();
    }
  }

  internal class RequireDoItRegistrar : RegistrarBase<IUnityContainer>
  {
    /// <inheritdoc />
    protected override void RegisterInternal(IUnityContainer container)
    {
      container.RegisterType<IRequireDoIt, RequireDoIt>();
    }
  }
  
  public class BundlingRegistrar : RegistrarBase<IUnityContainer>
  {
    /// <inheritdoc />
    protected override void RegisterInternal(IUnityContainer container)
    {
      container.RegisterRegistrar(new RequireDoItRegistrar());
      container.RegisterRegistrar(new DoItRegistrar());
    }
  }
  
  [DependencyInjectingTestFixture]
  public class HappyPathTests
  {
    private readonly IRequireDoIt _requireDoIt;
    private readonly IDoIt _doIt;

    public HappyPathTests(IRequireDoIt requireDoIt, IDoIt doIt)
    {
      _requireDoIt = requireDoIt;
      _doIt = doIt;
    }

    [Test]
    public void Unity_Extension_can_inject_multiple_dependencies()
    {
      Assert.That(_requireDoIt, Is.Not.Null);
      Assert.That(_doIt, Is.Not.Null);
    }

    [Test]
    public void Unity_Extension_can_inject_a_dependency_that_has_dependencies()
    {
      Assert.That(_requireDoIt, Is.Not.Null);
      Assert.That(_requireDoIt.DoIt, Is.Not.Null);
    }
  }
}