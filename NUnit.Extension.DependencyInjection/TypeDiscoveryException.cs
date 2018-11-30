using System;

#if NETFULL
using System.Runtime.Serialization;
#endif

namespace NUnit.Extension.DependencyInjection
{
#if NETFULL
  [Serializable]
#endif
  public class TypeDiscoveryException : Exception
  {
    public Type TypeDiscovererType { get; }

    /// <inheritdoc />
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
