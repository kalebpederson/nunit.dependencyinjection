// Copyright (c) Kaleb Pederson Software LLC. All rights reserved.
// Licensed under the MIT license. See LICENSE file alongside the solution file for full license information.

using System;
using System.Linq;
using NUnit.Extension.DependencyInjection.Abstractions;
using NUnit.Framework;
using Unity;

namespace NUnit.Extension.DependencyInjection.Unity.Tests
{
  [TestFixture]
  public class IocRegistrarTypeDiscovererTests
  {
    private class TestingNestedClass
    {
    }

    private class CallTrackingRegistrar : RegistrarBase<IUnityContainer>
    {
      public static int RegisterCallCount { get; set; }

      /// <inheritdoc />
      protected override void RegisterInternal(IUnityContainer container)
      {
        RegisterCallCount++;
      }
    }

    private class ActionInvokingRegistrar : RegistrarBase<IUnityContainer>
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
    
    [Test]
    public void Discover_runs_and_resolves_the_ActionInvokingRegistrar_Register_method()
    {
      using (var container = new UnityContainer())
      {
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
          $"Expected the registration action to be called and have registered the {typeof(TestingNestedClass).FullName} type.");
      }
    }

    [Test]
    public void Discover_runs_and_resolves_the_CallTrackingRegistrar_Register_method()
    {
      using (var container = new UnityContainer())
      {
        var callCount = 0;
        void RegistrationAction(IUnityContainer c) => callCount++;

        container.RegisterInstance((Action<IUnityContainer>)RegistrationAction);
        var discoverer = new IocRegistrarTypeDiscoverer();
        discoverer.Discover(container);

        Assert.That(CallTrackingRegistrar.RegisterCallCount, Is.GreaterThan(0));
      }
    }

    [Test]
    public void Discover_throws_TypeDiscoveryException_on_registrar_error()
    {
      using (var container = new UnityContainer())
      {
        void RegistrationAction(IUnityContainer c) => throw new Exception("Failed!");

        container.RegisterInstance((Action<IUnityContainer>) RegistrationAction);
        var discoverer = new IocRegistrarTypeDiscoverer();
        Assert.That(
          () => discoverer.Discover(container),
          Throws.Exception.TypeOf<TypeDiscoveryException>());
      }
    }
  }
}