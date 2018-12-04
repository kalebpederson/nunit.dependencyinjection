using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace NUnit.Extension.DependencyInjection.Unity.Tests
{
  public class TestSettings : ITestSettings
  {
    public IEnumerable<KeyValuePair<string, object>> GetAllSettings()
    {
      return TestContext.Parameters.Names.Select(x => new KeyValuePair<string, object>(x, TestContext.Parameters[x])).ToList();
    }
  }
}