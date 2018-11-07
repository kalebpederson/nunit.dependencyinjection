using System.Collections.Generic;

namespace NUnit.Extension.DependencyInjection.Tests
{
  public interface ITestSettings
  {
    IEnumerable<KeyValuePair<string, object>> GetAllSettings();
  }
}