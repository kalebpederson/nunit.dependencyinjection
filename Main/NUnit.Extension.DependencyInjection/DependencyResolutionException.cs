using System;

#if NETFULL
using System.Runtime.Serialization;
#endif

namespace NUnit.Extension.DependencyInjection
{
  /// <summary>
  /// Exception thrown when an error occurs attempting to resolve a dependency that
  /// needed to be injected into a test fixture.
  /// </summary>
#if NETFULL
  [Serializable]
#endif
  public class DependencyResolutionException : Exception
  {
    /// <summary>
    /// The type of class on which dependency injection was being performed.
    /// </summary>
    public Type InjectionClassType {get; }
    
    /// <summary>
    /// The type of parameter being injected into the <see cref="InjectionClassType"/> class.
    /// </summary>
    public Type InjectionParameterType {get; }

    /// <inheritdoc />
    public DependencyResolutionException(Type injectionClassType, Type injectionParameterType, Exception innerException) 
      : base(FormatMessage(injectionClassType, injectionParameterType), innerException)
    {
      InjectionClassType = injectionClassType;
      InjectionParameterType = injectionParameterType;
    }

    private static string FormatMessage(Type injectionClassType, Type injectionParameterType)
    {
      return $"Failed to resolve dependency of type {injectionParameterType.FullName} " +
             $"needed by class {injectionClassType.FullName}.";
    }

#if NETFULL
    /// <inheritdoc />
    protected DependencyResolutionException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
      InjectionClassType = (Type)info.GetValue(nameof(InjectionClassType), typeof(Type));
      InjectionParameterType = (Type)info.GetValue(nameof(InjectionParameterType), typeof(Type));
    }
#endif

  }
}