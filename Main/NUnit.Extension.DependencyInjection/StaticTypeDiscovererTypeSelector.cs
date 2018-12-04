using System;

namespace NUnit.Extension.DependencyInjection
{
  public class StaticTypeDiscovererTypeSelector : ITypeDiscovererTypeSelector
  {
    private readonly Type _type;

    public StaticTypeDiscovererTypeSelector(Type type)
    {
      _type = type;
    }
    /// <inheritdoc />
    public Type GetTypeDiscovererType()
    {
      return _type;
    }
  }
}