using System;

namespace NUnit.Extension.DependencyInjection
{
  public class StaticInjectionFactoryTypeSelector : IInjectionFactoryTypeSelector
  {
    private readonly Type _type;

    public StaticInjectionFactoryTypeSelector(Type type)
    {
      _type = type;
    }
    /// <inheritdoc />
    public Type GetInjectionType()
    {
      return _type;
    }
  }
}