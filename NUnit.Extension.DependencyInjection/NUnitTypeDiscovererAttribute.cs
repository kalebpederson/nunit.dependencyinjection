using System;

namespace NUnit.Extension.DependencyInjection
{
  [AttributeUsage(AttributeTargets.Assembly)]
  public class NUnitTypeDiscovererAttribute : Attribute
  {
    public Type[] TypeDiscovererTypes { get; }

    public NUnitTypeDiscovererAttribute(params Type[] typeDiscovererTypes)
    {
      TypeDiscovererValidator.AssertAreValidDiscovererTypes(typeDiscovererTypes);

      TypeDiscovererTypes = typeDiscovererTypes;
    }
  }
}