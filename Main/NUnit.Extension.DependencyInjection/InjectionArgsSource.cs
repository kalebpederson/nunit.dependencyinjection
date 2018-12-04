using System;
using System.Linq;
using System.Reflection;

namespace NUnit.Extension.DependencyInjection
{
  public class InjectionArgsSource<T> : InjectionArgsSource
  {
    public InjectionArgsSource(Func<Type, object> injectionFunc) 
      : base(injectionFunc, typeof(T))
    {
    }
  }

  public class InjectionArgsSource
  {
    private readonly Func<Type, object> _injectionFunc;
    private readonly Type _typeToInjectInto;
    private readonly ConstructorInfo _ctor;

    /// <summary>
    /// Creates an instance of the <see cref="InjectionArgsSource"/>. 
    /// </summary>
    /// <remarks>
    /// The <paramref name="injectionFunc"/> must be thread safe.
    /// </remarks>
    /// <param name="injectionFunc">Function creating the type to be injected.</param>
    /// <param name="typeToInjectInto">The type into which the arguments will be injected.</param>
    public InjectionArgsSource(Func<Type, object> injectionFunc, Type typeToInjectInto)
    {
      AssertIsNotNull(nameof(injectionFunc), injectionFunc);
      AssertIsNotNull(nameof(typeToInjectInto), typeToInjectInto);
      AssertIsConstructableType(typeToInjectInto);
      _injectionFunc = injectionFunc;
      _typeToInjectInto = typeToInjectInto;
      _ctor = GetConstructorFor(typeToInjectInto);
    }

    private static ConstructorInfo GetConstructorFor(Type typeToInjectInto)
    {
      return GetConstructors(typeToInjectInto).Single();
    }

    private static void AssertIsConstructableType(Type typeToInjectInto)
    {
      AssertIsNotAbstract(typeToInjectInto);
      AssertIsNotOpenGenericType(typeToInjectInto);
      AssertHasSinglePublicConstructor(typeToInjectInto);
    }

    private static void AssertIsNotOpenGenericType(Type typeToInjectInto)
    {
      if (typeToInjectInto.IsGenericType && !typeToInjectInto.IsConstructedGenericType)
      {
        throw new ArgumentOutOfRangeException(
          nameof(typeToInjectInto),
          $"Type {typeToInjectInto.FullName} must be a constructable generic type."
          );
      }
    }

    private static void AssertHasSinglePublicConstructor(Type typeToInjectInto)
    {
      var constructors = GetConstructors(typeToInjectInto);
      if (constructors == null || constructors.Length != 1)
      {
        throw new ArgumentOutOfRangeException(
          nameof(typeToInjectInto), $"Type {typeToInjectInto.FullName} must have a single public constructor."
          );
      }
    }

    private static ConstructorInfo[] GetConstructors(Type typeToInjectInto)
    {
      return typeToInjectInto.GetConstructors(BindingFlags.Public | BindingFlags.Instance | BindingFlags.CreateInstance);
    }

    private static void AssertIsNotAbstract(Type typeToInjectInto)
    {
      if (typeToInjectInto.IsAbstract)
      {
        throw new ArgumentOutOfRangeException(
          nameof(typeToInjectInto), $"Type {typeToInjectInto.FullName} is an abstract type and cannot be constructed.");
      }
    }

    private void AssertIsNotNull(string name, object value)
    {
      if (value == null) throw new ArgumentNullException(name, $"Parameter {name} was null");
    }

    public object[] GetInjectionParameters()
    {
      return 
        _ctor.GetParameters()
          .Select(x => x.ParameterType)
          .Select(TryInject)
          .ToArray();
    }
    
    private object TryInject(Type injectionType)
    {
      try
      {
        return _injectionFunc(injectionType);
      }
      catch (Exception ex)
      {
        throw new DependencyResolutionException(_typeToInjectInto, injectionType, ex);
      }
    }
  }
}