using System;

namespace NUnit.Extension.DependencyInjection
{
  public interface IInjectionFactory
  {
    // TODO: Move to separate interface: IInjectionFactoryInitializer ?
    void Initialize(ITypeDiscoverer typeDiscoverer);
    object Create(Type type);
  }
}