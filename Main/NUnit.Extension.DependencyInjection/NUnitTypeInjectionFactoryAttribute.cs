using System;

namespace NUnit.Extension.DependencyInjection
{
  [AttributeUsage(AttributeTargets.Assembly)]
  public class NUnitTypeInjectionFactoryAttribute : Attribute
  {
    public Type InjectionFactoryType { get; }

    public NUnitTypeInjectionFactoryAttribute(Type factoryType)
    {
      InjectionFactoryTypeValidator.AssertIsValidFactoryType(factoryType);

      InjectionFactoryType = factoryType;
    }
  }
}