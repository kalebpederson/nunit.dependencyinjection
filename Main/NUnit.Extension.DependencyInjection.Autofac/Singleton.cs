namespace NUnit.Extension.DependencyInjection.Autofac
{
  internal static class Singleton<T> where T : class, new()
  {
    // ReSharper disable once StaticMemberInGenericType
    private static readonly object _syncLock = new object();

    private static T _instance = null;

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