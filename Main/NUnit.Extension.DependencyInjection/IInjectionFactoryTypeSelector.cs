using System;

namespace NUnit.Extension.DependencyInjection
{
  public interface IInjectionFactoryTypeSelector
  {
    Type GetInjectionType();
  }
}