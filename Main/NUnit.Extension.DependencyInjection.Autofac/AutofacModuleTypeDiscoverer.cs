using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Autofac;

namespace NUnit.Extension.DependencyInjection.Autofac
{
  public class AutofacModuleTypeDiscoverer : TypeDiscovererBase<ContainerBuilder>
  {
    protected override void DiscoverInternal(ContainerBuilder containerBuilder)
    {
      var types = AppDomain.CurrentDomain.GetAssemblies()
        .Where(AssemblyCanBeLoaded)
        .SelectMany(a => a.GetTypes())
        .Where(t => 
          typeof(IIocRegistrar).IsAssignableFrom(t) 
          && !t.IsAbstract)
        .ToList();
      foreach (var registrar in types)
      {
        ResolveAndRunRegistrar(containerBuilder, registrar);
      }
    }

    private static void ResolveAndRunRegistrar(ContainerBuilder containerBuilder, Type registrar)
    {
      try
      {
        var registrarInstance = (IIocRegistrar) Activator.CreateInstance(registrar);
        registrarInstance.Register(containerBuilder);
      }
      catch (Exception ex)
      {
        throw new TypeDiscoveryException(registrar, ex);
      }
    }

    private static bool AssemblyCanBeLoaded(Assembly assembly)
    {
      try
      {
        return assembly.GetTypes().Any();
      }
      catch (ReflectionTypeLoadException ex)
      {
        // This isn't our fault, and there's nothing we can do with about it.
        // Erroring out and aborting all loading isn't going to help up so
        // it is only reasonable to skip these assemblies.
        Trace.TraceWarning($"Unable to load assembly {assembly.FullName}: {ex.Message}");
        return false;
      }
    }
  }
}
