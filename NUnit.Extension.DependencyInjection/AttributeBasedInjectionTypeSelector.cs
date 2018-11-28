using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace NUnit.Extension.DependencyInjection
{
  public class AttributeBasedInjectionTypeSelector : IInjectionTypeSelector
  {
    /// <inheritdoc />
    public Type GetInjectionType()
    {
      return GetInjectionFactoryTypeFromAttribute();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Type GetInjectionFactoryTypeFromAttribute()
    {
      var injectionFactoryAttribute = AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(a => a.GetCustomAttributes(typeof(NUnitTypeInjectionFactoryAttribute), false))
        .OfType<NUnitTypeInjectionFactoryAttribute>()
        .FirstOrDefault();
      if (injectionFactoryAttribute == null)
      {
        throw new InvalidOperationException(
          $"{nameof(DependencyInjectingTestFixtureAttribute)} requires an injection plugin be loaded. Please ensure " +
          $"that one is present or create one using the {typeof(IIocContainer).FullName} interface " +
          $"and register it using the {typeof(NUnitTypeInjectionFactoryAttribute).FullName} attribute.");
      }
      InjectionFactoryTypeValidator.AssertIsValidFactoryType(injectionFactoryAttribute.InjectionFactoryType);
      Trace.TraceInformation(
        $"Found {nameof(NUnitTypeInjectionFactoryAttribute)} in assembly " +
        $"{injectionFactoryAttribute.GetType().Assembly.FullName}. Will use the {injectionFactoryAttribute.InjectionFactoryType} type " +
        "to create dependencies.");
      return injectionFactoryAttribute.InjectionFactoryType;
    }
  }
}