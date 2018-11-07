using System;
using System.Linq;
using Unity;

namespace NUnit.Extension.DependencyInjection.Unity
{
  public class IocRegistrarTypeDiscoverer : ITypeDiscoverer
  {
    private readonly IUnityContainer _container;

    public IocRegistrarTypeDiscoverer(IUnityContainer container)
    {
      _container = container;
    }

    public void Discover()
    {
      var types = AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(a => a.GetTypes())
        .Where(t => typeof(IIocRegistrar).IsAssignableFrom(t))
        .ToList();
      foreach (var registrar in types)
      {
        // exception handling
        var registrarInstance = (IIocRegistrar)_container.Resolve(registrar);
        registrarInstance.Register(_container);
      }
    }
  }
}