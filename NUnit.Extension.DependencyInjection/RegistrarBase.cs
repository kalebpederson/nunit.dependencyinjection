using System;

namespace NUnit.Extension.DependencyInjection
{
  public abstract class RegistrarBase<T> : IIocRegistrar
  {
    /// <inheritdoc />
    public void Register(object container)
    {
      if (!(container is T))
      {
        throw new ArgumentOutOfRangeException(
          $"{nameof(container)} should have been of type {typeof(T).FullName} " +
          $"but was of type {container.GetType().FullName}"
        );
      }
      RegisterInternal((T)container);
    }
    
    protected abstract void RegisterInternal(T container);
  }
}