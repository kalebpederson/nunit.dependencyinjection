using System;
using System.Linq;
using Unity;
using Unity.RegistrationByConvention;

namespace NUnit.Extension.DependencyInjection.Unity
{
  public class ConventionMappingTypeDiscoverer : TypeDiscovererBase<IUnityContainer>
  {
    protected override void DiscoverInternal(IUnityContainer container)
    {
      try
      {
        container.RegisterTypes(
          AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => a.GetCustomAttributes(typeof(ScanInContainerAttribute), true).Any())
            .SelectMany(x => x.GetTypes()),
          WithMappings.FromMatchingInterface,
          WithName.Default,
          WithLifetime.Hierarchical
        );
      }
      catch (Exception ex)
      {
        throw new TypeDiscoveryException(GetType(), ex);
      }
    }
  }
}