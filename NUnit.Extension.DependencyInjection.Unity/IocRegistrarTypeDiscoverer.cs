using System;
using System.Linq;
using Unity;

namespace NUnit.Extension.DependencyInjection.Unity
{
  public class IocRegistrarTypeDiscoverer : TypeDiscovererBase<IUnityContainer>
  {
    protected override void DiscoverInternal(IUnityContainer container)
    {
      var types = AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(a => a.GetTypes())
        .Where(t => 
          typeof(IIocRegistrar).IsAssignableFrom(t) 
          && !t.IsAbstract)
        .ToList();
      foreach (var registrar in types)
      {
        // TODO: exception handling
        var registrarInstance = (IIocRegistrar)container.Resolve(registrar);
        registrarInstance.Register(container);
      }
    }
  }
}