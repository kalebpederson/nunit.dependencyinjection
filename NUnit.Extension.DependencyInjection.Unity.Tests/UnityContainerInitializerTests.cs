using System;
using NUnit.Framework;

namespace NUnit.Extension.DependencyInjection.Unity.Tests
{
  [TestFixture]
  public class UnityContainerInitializerTests
  {
    [Test]
    public void TypeDiscoverer_is_called_during_initialization()
    {
      WithRestoreTypeDiscoverer(() =>
      {
        var called = 0;
        void Initialize() => called++;
        UnityContainerInitializer.TypeDiscoverer = Initialize;
        UnityContainerInitializer.Initialize();
        Assert.That(called, Is.EqualTo(1));
      });
    }

    [Test]
    public void InitializeAction_is_called_during_call_to_Initialize()
    {
      WithRestoreInitializeAction(() =>
      {
        var called = 0;
        void Initialize() => called++;
        UnityContainerInitializer.InitializeAction = Initialize;
        UnityContainerInitializer.Initialize();
        Assert.That(called, Is.EqualTo(1));
      });
    }

    private void WithRestoreTypeDiscoverer(Action action)
    {
      var originalInstance = UnityContainerInitializer.TypeDiscoverer;
      try
      {
        action.Invoke();
      }
      catch (Exception)
      {
        UnityContainerInitializer.TypeDiscoverer = originalInstance;
        throw;
      }
    }

    private void WithRestoreInitializeAction(Action action)
    {
      var originalInstance = UnityContainerInitializer.InitializeAction;
      try
      {
        action.Invoke();
      }
      catch (Exception)
      {
        UnityContainerInitializer.InitializeAction = originalInstance;
        throw;
      }
    }
  }
}