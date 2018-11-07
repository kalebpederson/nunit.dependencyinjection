using System;

namespace NUnit.Extension.DependencyInjection
{
  public interface IInjectionFactory
  {
    object Create(Type type);
  }
}