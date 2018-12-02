using System;

namespace NUnit.Extension.DependencyInjection
{
  [AttributeUsage(AttributeTargets.Assembly)]
  public class NUnitTypeDiscovererAttribute : Attribute
  {
    public Type TypeDiscovererType { get; }

    public NUnitTypeDiscovererAttribute(Type typeDiscovererType)
    {
      TypeDiscovererTypeValidator.AssertIsValidDiscovererType(typeDiscovererType);

      TypeDiscovererType = typeDiscovererType;
    }
  }
}