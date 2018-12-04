using System;

namespace NUnit.Extension.DependencyInjection.Unity
{
  internal static class FunctionalHelpers
  {
    public static Action<T> CreateRunOnce<T>(Action<T> action)
    {
      var executed = false;
      var syncLock = new object();
      return t =>
      {
        if (!executed)
        {
          lock (syncLock)
          {
            if (!executed)
            {
              action.Invoke(t);
              executed = true;
            }
          }
        }
      };
    }

  }
}