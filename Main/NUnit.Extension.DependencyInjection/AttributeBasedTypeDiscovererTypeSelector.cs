using System;
using System.Diagnostics;
using System.Linq;

namespace NUnit.Extension.DependencyInjection
{
  public class AttributeBasedTypeDiscovererTypeSelector : ITypeDiscovererTypeSelector
  {
    /// <inheritdoc />
    public Type GetTypeDiscovererType()
    {
      var typeDiscovererAttribute = AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(a => a.GetCustomAttributes(typeof(NUnitTypeDiscovererAttribute), false))
        .OfType<NUnitTypeDiscovererAttribute>()
        .FirstOrDefault();
      if (typeDiscovererAttribute == null)
      {
        throw new InvalidOperationException(
          $"The {typeof(ITypeDiscoverer).FullName} registered requires the presence of at least one type " +
          $"discoverer but none were found.");
      }
      TypeDiscovererTypeValidator.AssertIsValidDiscovererType(typeDiscovererAttribute.TypeDiscovererType);
      Trace.TraceInformation(
        $"Found {nameof(NUnitTypeDiscovererAttribute)} in assembly " +
        $"{typeDiscovererAttribute.GetType().Assembly.FullName}. Will use the type discoverer of type " +
        $"{typeDiscovererAttribute.TypeDiscovererType.FullName}.");
      return typeDiscovererAttribute.TypeDiscovererType;
    }
  }
}