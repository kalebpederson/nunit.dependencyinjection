using System;

namespace NUnit.Extension.DependencyInjection
{
  public interface IIocContainer
  {
    object Create(Type type);
  }
}