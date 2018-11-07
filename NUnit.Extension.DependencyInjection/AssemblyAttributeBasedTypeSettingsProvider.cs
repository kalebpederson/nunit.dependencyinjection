using System;
using System.Diagnostics;
using System.Linq;

namespace NUnit.Extension.DependencyInjection
{
  public class AssemblyAttributeBasedTypeSettingsProvider
  {
    /// <inheritdoc />
    public Type GetInjectionFactoryType()
    {
      var injectionFactoryAttribute = AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(a => a.GetCustomAttributes(typeof(NUnitTypeInjectionFactoryAttribute), false))
        .OfType<NUnitTypeInjectionFactoryAttribute>()
        .FirstOrDefault();
      if (injectionFactoryAttribute == null)
      {
        throw new InvalidOperationException(
          $"{nameof(DependencyInjectingTestFixtureAttribute)} requires an injection plugin be loaded. Please ensure " +
          $"that one is present or create one using the {typeof(IInjectionFactory).FullName} interface " +
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