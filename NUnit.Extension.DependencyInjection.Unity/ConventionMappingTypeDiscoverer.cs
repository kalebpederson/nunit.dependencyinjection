using System;
using System.Linq;
using Unity;
using Unity.RegistrationByConvention;

namespace NUnit.Extension.DependencyInjection.Unity
{
  public class ConventionMappingTypeDiscoverer : ITypeDiscoverer
  {
    private readonly IUnityContainer _container;

    public ConventionMappingTypeDiscoverer(IUnityContainer container)
    {
      _container = container;
    }

    public void Discover()
    {
      _container.RegisterTypes(
        AppDomain.CurrentDomain.GetAssemblies()
          .Where(a => a.GetCustomAttributes(typeof(ScanInContainerAttribute), true).Any())
          .SelectMany(x => x.GetTypes()),
        WithMappings.FromMatchingInterface,
        WithName.Default,
        WithLifetime.Hierarchical
      );
    }
  }
}