// Copyright (c) Kaleb Pederson Software LLC. All rights reserved.
// Licensed under the MIT license. See LICENSE file alongside the solution file for full license information.

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace NUnit.Extension.DependencyInjection.Unity.Tests.SampleTypes
{
  public class TestSettings : ITestSettings
  {
    public IEnumerable<KeyValuePair<string, object>> GetAllSettings()
    {
      return TestContext.Parameters.Names.Select(x => new KeyValuePair<string, object>(x, TestContext.Parameters[x])).ToList();
    }
  }
}