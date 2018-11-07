using System;
using Unity;

namespace NUnit.Extension.DependencyInjection.Unity
{
  public class UnityInjectionFactory : IInjectionFactory
  {
    public UnityInjectionFactory()
    {
      UnityContainerInitializer.Initialize();
    }

    /// <inheritdoc />
    public object Create(Type type)
    {
      return UnityContainerHolder.Instance.Resolve(type);
    }
  }
}