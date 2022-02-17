// Copyright (c) Kaleb Pederson Software LLC. All rights reserved.
// Licensed under the MIT license. See LICENSE file alongside the solution file for full license information.

using System.Collections.Generic;

namespace NUnit.Extension.DependencyInjection.Unity.Tests.SampleTypes
{
  public interface ITestSettings
  {
    IEnumerable<KeyValuePair<string, object>> GetAllSettings();
  }
}