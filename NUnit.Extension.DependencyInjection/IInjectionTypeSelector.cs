using System;

namespace NUnit.Extension.DependencyInjection
{
  public interface IInjectionTypeSelector
  {
    Type GetInjectionType();
  }
}