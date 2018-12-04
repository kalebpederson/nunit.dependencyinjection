using System.Collections.Generic;

namespace NUnit.Extension.DependencyInjection.Unity.Tests
{
  public interface ITestSettings
  {
    IEnumerable<KeyValuePair<string, object>> GetAllSettings();
  }
}