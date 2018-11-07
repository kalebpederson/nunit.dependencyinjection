using System;
using Unity;

namespace NUnit.Extension.DependencyInjection.Unity
{
  public static class UnityContainerHolder
  {
    internal static Func<IUnityContainer> InstanceFactory { get; set; } = () => new UnityContainer();
    public static IUnityContainer Instance { get; internal set; } = InstanceFactory();
  }
}