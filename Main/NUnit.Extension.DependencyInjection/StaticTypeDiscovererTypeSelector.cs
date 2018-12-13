using System;

namespace NUnit.Extension.DependencyInjection
{
  /// <summary>
  /// <see cref="ITypeDiscovererTypeSelector"/> that will always return the
  /// type provided at the time of construction of this type selector.
  /// </summary>
  public class StaticTypeDiscovererTypeSelector : ITypeDiscovererTypeSelector
  {
    private readonly Type _type;

    /// <summary>
    /// Creates an instance of the type selector.
    /// </summary>
    /// <param name="type">The <see cref="ITypeDiscoverer"/> to be used.</param>
    public StaticTypeDiscovererTypeSelector(Type type)
    {
      _type = type;
    }

    /// <inheritdoc />
    public Type GetTypeDiscovererType()
    {
      return _type;
    }
  }
}