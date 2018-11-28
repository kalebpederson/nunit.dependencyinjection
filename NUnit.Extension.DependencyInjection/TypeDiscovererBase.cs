namespace NUnit.Extension.DependencyInjection
{
  public abstract class TypeDiscovererBase<T> : ITypeDiscoverer
  {
    /// <inheritdoc />
    public void Discover(object container)
    {
      DiscoverInternal((T) container);
    }

    protected abstract void DiscoverInternal(T container);
  }
}