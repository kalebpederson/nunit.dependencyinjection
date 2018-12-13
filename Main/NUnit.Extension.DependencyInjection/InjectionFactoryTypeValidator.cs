using System;
using System.Reflection;

namespace NUnit.Extension.DependencyInjection
{
  /// <summary>
  /// Provides a means of validating that the injection type is valid.
  /// </summary>
  public static class InjectionFactoryTypeValidator
  {
    /// <summary>
    /// Validates that the injection type is valid. In other words,
    /// that the following conditions are met:
    /// <list type="bullet">
    /// <item><paramref name="factoryType"/> is not null</item>
    /// <item>The type implements the <see cref="IInjectionFactory"/> interface</item>
    /// <item>The type has a public no-args constructor</item>
    /// </list>
    /// </summary>
    /// <param name="factoryType">The factory type to be validated.</param>
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