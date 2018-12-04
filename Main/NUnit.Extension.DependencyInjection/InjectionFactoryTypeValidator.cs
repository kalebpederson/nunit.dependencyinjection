using System;
using System.Reflection;

namespace NUnit.Extension.DependencyInjection
{
  public static class InjectionFactoryTypeValidator
  {
    public static void AssertIsValidFactoryType(Type factoryType)
    {
      AssertIsNotNull(factoryType);
      AssertImplementsProperInterface(factoryType);
      AssertHasPublicNoArgsConstructor(factoryType);
    }

    internal static void AssertIsNotNull(Type factoryType)
    {
      if (factoryType == null)
      {
        throw new ArgumentNullException(
          $"{nameof(factoryType)} specified as {nameof(IInjectionFactory)} on " +
          $"{nameof(NUnitTypeInjectionFactoryAttribute)} cannot be null."
        );
      }
    }

    internal static void AssertHasPublicNoArgsConstructor(Type factoryType)
    {
      var ctorInfo = factoryType.GetConstructor(
        BindingFlags.Public | BindingFlags.Instance,
        null, CallingConventions.Standard, Type.EmptyTypes, null);
      if (ctorInfo == null)
      {
        throw new ArgumentOutOfRangeException(
          nameof(factoryType),
          $"{factoryType.FullName} specified as {nameof(IInjectionFactory)} on " +
          $"{nameof(NUnitTypeInjectionFactoryAttribute)} must have a public no-args constructor."
        );
      }
    }

    internal static void AssertImplementsProperInterface(Type factoryType)
    {
      if (!typeof(IInjectionFactory).IsAssignableFrom(factoryType))
      {
        throw new ArgumentOutOfRangeException(
          $"{nameof(factoryType)} specified on {nameof(NUnitTypeInjectionFactoryAttribute)} " +
          $"must be of type {nameof(IInjectionFactory)}."
        );
      }
    }
  }
}