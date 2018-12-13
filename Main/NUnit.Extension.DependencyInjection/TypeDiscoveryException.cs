using System;

#if NETFULL
using System.Runtime.Serialization;
#endif

namespace NUnit.Extension.DependencyInjection
{
  /// <summary>
  /// Exception that is throw during failed type discovery.
  /// </summary>
#if NETFULL
  [Serializable]
#endif
  public class TypeDiscoveryException : Exception
  {
    /// <summary>
    /// The type of the <see cref="ITypeDiscoverer"/> that failed.
    /// </summary>
    public Type TypeDiscovererType { get; }

    /// <inheritdoc />
    /// <summary>
    /// Creates an instance of the exception that occurred while
    /// processing an instance of <see cref="erringTypeDiscovererType"/>
    /// which resulted in the exception specified by <paramref name="innerException"/>.
    /// </summary>
    public TypeDiscoveryException(Type erringTypeDiscovererType, Exception innerException) 
      : base(FormatMessage(erringTypeDiscovererType), innerException)
    {
      TypeDiscovererType = erringTypeDiscovererType;
    }

    private static string FormatMessage(Type erringTypeDiscovererType)
    {
      return $"{nameof(ITypeDiscoverer)} of type {erringTypeDiscovererType.FullName} failed.";
    }

#if NETFULL
    /// <inheritdoc />
    protected TypeDiscoveryException(SerializationInfo info, StreamingContext context) 
      : base(info, context)
    {
      TypeDiscovererType = (Type)info.GetValue(nameof(TypeDiscovererType), typeof(Type));
    }
#endif
  }
}
