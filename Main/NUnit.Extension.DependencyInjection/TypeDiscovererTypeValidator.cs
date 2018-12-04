using System;
using System.Reflection;

namespace NUnit.Extension.DependencyInjection
{
  public static class TypeDiscovererTypeValidator
  {
    public static void AssertIsValidDiscovererType(Type discovererType)
    {
      AssertIsNotNull(discovererType);
      AssertImplementsProperInterface(discovererType);
      AssertHasPublicNoArgsConstructor(discovererType);
    }

    internal static void AssertIsNotNull(Type discovererType)
    {
      if (discovererType == null)
      {
        throw new ArgumentNullException(
          $"{nameof(discovererType)} specified as {nameof(ITypeDiscoverer)} on " +
          $"{nameof(NUnitTypeDiscovererAttribute)} cannot be null."
        );
      }
    }

    internal static void AssertHasPublicNoArgsConstructor(Type discovererType)
    {
      var ctorInfo = discovererType.GetConstructor(
        BindingFlags.Public | BindingFlags.Instance,
        null, CallingConventions.Standard, Type.EmptyTypes, null);
      if (ctorInfo == null)
      {
        throw new ArgumentOutOfRangeException(
          nameof(discovererType),
          $"{discovererType.FullName} specified as {nameof(ITypeDiscoverer)} on " +
          $"{nameof(NUnitTypeDiscovererAttribute)} must have a public no-args constructor."
        );
      }
    }

    internal static void AssertImplementsProperInterface(Type discovererType)
    {
      if (!typeof(ITypeDiscoverer).IsAssignableFrom(discovererType))
      {
        throw new ArgumentOutOfRangeException(
          $"{nameof(discovererType)} specified on {nameof(NUnitTypeDiscovererAttribute)} " +
          $"must be of type {nameof(ITypeDiscoverer)}."
        );
      }
    }
  }
}