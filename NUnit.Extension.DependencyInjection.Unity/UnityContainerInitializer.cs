using System;

namespace NUnit.Extension.DependencyInjection.Unity
{
  public static class UnityContainerInitializer
  {
    private static readonly object _syncLock = new Object();

    private static Action _typeDiscoverer =
      () => new ConventionMappingTypeDiscoverer(UnityContainerHolder.Instance).Discover();

    internal static Action TypeDiscoverer
    {
      get => _typeDiscoverer;
      set
      {
        lock (_syncLock)
        {
          _typeDiscoverer = value;
          IsInitialized = false;
        }
      }
    }

    private static void InitializeActionInternal()
    {
      if (!IsInitialized)
      {
        lock (_syncLock)
        {
          if (!IsInitialized)
          {
            TypeDiscoverer();
            IsInitialized = true;
          }
        }
      }
    }

    private static Action _initializeAction = InitializeActionInternal;
    internal static Action InitializeAction
    {
      get => _initializeAction;
      set
      {
        lock (_syncLock)
        {
          _initializeAction = value;
          IsInitialized = false;
        }
      }
    }

    public static bool IsInitialized { get; internal set; } = false;
    public static void Initialize() => InitializeAction();
  }
}