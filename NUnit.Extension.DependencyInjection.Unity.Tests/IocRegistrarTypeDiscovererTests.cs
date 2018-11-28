using System;
using System.Linq;
using NUnit.Framework;
using Unity;

namespace NUnit.Extension.DependencyInjection.Unity.Tests
{
  [TestFixture]
  public class IocRegistrarTypeDiscovererTests
  {
    public class TestingNestedClass {}

    public class ActionInvokingRegistrar : RegistrarBase<IUnityContainer>
    {
      private readonly Action<IUnityContainer> _action;

      public ActionInvokingRegistrar(Action<IUnityContainer> action)
      {
        _action = action;
      }

      /// <inheritdoc />
      protected override void RegisterInternal(IUnityContainer container)
      {
        _action.Invoke(container);
      }
    }

    public class CallTrackingRegistrar : RegistrarBase<IUnityContainer>
    {
      public static int RegisterCallCount { get; set; }

      /// <inheritdoc />
      protected override void RegisterInternal(IUnityContainer container)
      {
        RegisterCallCount++;
      }
    }

    [Test]
    public void Discoverer_resolves_the_available_registrars()
    {
      var container = new UnityContainer();
      var callCount = 0;

      void RegistrationAction(IUnityContainer c)
      {
        callCount++;
        c.RegisterType<TestingNestedClass>();
      }

      container.RegisterInstance((Action<IUnityContainer>) RegistrationAction);
      var discoverer = new IocRegistrarTypeDiscoverer();
      discoverer.Discover(container);
      Assert.That(callCount, Is.EqualTo(1));
      Assert.That(
        container.Registrations.Any(r => r.RegisteredType == typeof(TestingNestedClass)),
        Is.True,
        $"Expected the registration action to be called and have registered the {typeof(TestingNestedClass).FullName} type."
      );
      
    }

    [Test]
    public void Discoverer_calls_the_CallTrackingRegistrar_Register_method()
    {
      var container = new UnityContainer();
      var callCount = 0;
      void RegistrationAction(IUnityContainer c) { callCount++; }

      container.RegisterInstance((Action<IUnityContainer>) RegistrationAction);
      var discoverer = new IocRegistrarTypeDiscoverer();
      discoverer.Discover(container);

      Assert.That(CallTrackingRegistrar.RegisterCallCount, Is.GreaterThan(0));
    }

  }
}