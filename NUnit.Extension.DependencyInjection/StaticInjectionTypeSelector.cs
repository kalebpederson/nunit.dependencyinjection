using System;

namespace NUnit.Extension.DependencyInjection
{
  public class StaticInjectionTypeSelector : IInjectionTypeSelector
  {
    private readonly Type _type;

    public StaticInjectionTypeSelector(Type type)
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