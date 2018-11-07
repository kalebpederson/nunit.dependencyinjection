﻿using System;
using System.Runtime.Serialization;

namespace NUnit.Extension.DependencyInjection
{
  [Serializable]
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

    /// <inheritdoc />
    protected DependencyResolutionException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
      InjectionClassType = (Type)info.GetValue(nameof(InjectionClassType), typeof(Type));
      InjectionParameterType = (Type)info.GetValue(nameof(InjectionParameterType), typeof(Type));
    }

  }
}