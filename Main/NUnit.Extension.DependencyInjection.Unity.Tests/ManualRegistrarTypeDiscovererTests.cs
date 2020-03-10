// Copyright (c) Kaleb Pederson Software LLC. All rights reserved.
// Licensed under the MIT license. See LICENSE file alongside the solution file for full license information.

using System;
using System.Linq;
using NUnit.Framework;
using Unity;

namespace NUnit.Extension.DependencyInjection.Unity.Tests
{
  [TestFixture]
  public class ManualRegistrarTypeDiscovererTests
  {
    private class TestingNestedClass
    {
    }
    
    [NUnitExcludeFromAutoScan]
    private class PostConstructionActionInvokingRegistrar : ActionInvokingRegistrar
    {
      public Action<IUnityContainer> Action { get => _action; set => _action = value; }
      
      /// <inheritdoc />
      public PostConstructionActionInvokingRegistrar() : base(null)
      {
      }
    }
    
    [NUnitExcludeFromAutoScan]
    private class ActionInvokingRegistrar : RegistrarBase<IUnityContainer>
    {
      protected Action<IUnityContainer> _action;

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
    
    [NUnitExcludeFromAutoScan]
    private class OpenGenericActionInvokingRegistrar<T> : RegistrarBase<IUnityContainer>
    {
      protected Action<IUnityContainer> _action;

      public OpenGenericActionInvokingRegistrar(Action<IUnityContainer> action)
      {
        _action = action;
      }

      /// <inheritdoc />
      protected override void RegisterInternal(IUnityContainer container)
      {
        _action.Invoke(container);
      }
    }
    
    [NUnitExcludeFromAutoScan]
    private class ClosedGenericActionInvokingRegistrar : OpenGenericActionInvokingRegistrar<int>
    {
      public ClosedGenericActionInvokingRegistrar(Action<IUnityContainer> action) : base(action)
      {
      }
    }
    
    [Test]
    public void Discover_runs_and_resolves_the_specified_registrar()
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
        var discoverer = new ManualRegistrarTypeDiscoverer(typeof(ActionInvokingRegistrar));
        discoverer.Discover(container);
        Assert.That(callCount, Is.EqualTo(1));
        Assert.That(
          container.Registrations.Any(r => r.RegisteredType == typeof(TestingNestedClass)),
          Is.True,
          $"Expected the registration action to be called and have registered the {typeof(TestingNestedClass).FullName} type.");
      }
    }
    
    [Test]
    public void Discover_throws_TypeDiscoveryException_on_registrar_discovery_error()
    {
      using (var container = new UnityContainer())
      {
        void RegistrationAction(IUnityContainer c) => throw new Exception("Failed!");

        container.RegisterInstance((Action<IUnityContainer>) RegistrationAction);
        var discoverer = new ManualRegistrarTypeDiscoverer(typeof(ActionInvokingRegistrar));
        Assert.That(
          () => discoverer.Discover(container),
          Throws.Exception.TypeOf<TypeDiscoveryException>());
      }
    }
    
    [Test]
    public void Discover_throws_TypeDiscoveryException_on_registrar_resolution_error()
    {
      using (var container = new UnityContainer())
      {
        var discoverer = new ManualRegistrarTypeDiscoverer(typeof(ActionInvokingRegistrar));
        Assert.That(
          () => discoverer.Discover(container),
          Throws.Exception.TypeOf<TypeDiscoveryException>());
      }
    }

    [Test]
    public void Discover_accepts_closed_generic_type_through_inheritance()
    {
      using (var container = new UnityContainer())
      {
        var callCount = 0;
        void RegistrationAction(IUnityContainer c) => callCount++;
        container.RegisterInstance((Action<IUnityContainer>) RegistrationAction);
        var discoverer = new ManualRegistrarTypeDiscoverer(typeof(ClosedGenericActionInvokingRegistrar));
        Assert.That(
          () => discoverer.Discover(container),
          Throws.Nothing);
        Assert.That(callCount, Is.EqualTo(1));
      }
    }
    
    [Test]
    public void Discover_accepts_closed_generic_type_through_explicit_type_specification()
    {
      using (var container = new UnityContainer())
      {
        var callCount = 0;
        void RegistrationAction(IUnityContainer c) => callCount++;
        container.RegisterInstance((Action<IUnityContainer>) RegistrationAction);
        var discoverer = new ManualRegistrarTypeDiscoverer(typeof(OpenGenericActionInvokingRegistrar<double>));
        Assert.That(
          () => discoverer.Discover(container),
          Throws.Nothing);
        Assert.That(callCount, Is.EqualTo(1));
      }
    }
    
    
    [Test]
    public void Discover_throws_TypeDiscoveryException_when_registered_type_is_open_generic_type()
    {
      using (var container = new UnityContainer())
      {
        var discoverer = new ManualRegistrarTypeDiscoverer(typeof(OpenGenericActionInvokingRegistrar<>));
        Assert.That(
          () => discoverer.Discover(container),
          Throws.Exception.TypeOf<TypeDiscoveryException>());
      }
    }
  }
}