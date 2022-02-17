// Copyright (c) Kaleb Pederson Software LLC. All rights reserved.
// Licensed under the MIT license. See LICENSE file alongside the solution file for full license information.

namespace NUnit.Extension.DependencyInjection.Unity
{
  internal static class Singleton<T> where T : class, new()
  {
    // ReSharper disable once StaticMemberInGenericType
    private static readonly object _syncLock = new object();

    private static T _instance;

    public static T Instance
    {
      get
      {
        if (_instance == null)
        {
          lock (_syncLock)
          {
            if (_instance == null)
            {
              _instance = new T();
              return _instance;
            }
          }
        }

        return _instance;
      }
    }
  }
}