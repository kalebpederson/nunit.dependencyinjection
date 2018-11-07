using System;
using NUnit.Framework;
using Unity;

namespace NUnit.Extension.DependencyInjection.Unity.Tests
{
  [TestFixture]
  public class UnityContainerHolderTests
  {
    [Test]
    public void InstanceFactory_returns_a_new_instance_of_IUnityContainer()
    {
      var c = UnityContainerHolder.InstanceFactory();
      Assert.That(c, Is.Not.Null);
      Assert.That(c, Is.InstanceOf<IUnityContainer>());
    }

    [Test]
    public void InstanceFactory_can_be_set_for_testing_purposes()
    {
      WithInstanceFactoryRestored(() =>
      {
        int called = 0;
        var container = new UnityContainer();
        Func<IUnityContainer> factory = () =>
        {
          called++;
          return container;
        };
        UnityContainerHolder.InstanceFactory = factory;
        var c = UnityContainerHolder.InstanceFactory();
        Assert.That(called, Is.EqualTo(1));
      });
    }

    [Test]
    public void Instance_can_be_set_for_testing_purposes()
    {
      WithInstanceRestored(()=>
      {
        var container = new UnityContainer();
        UnityContainerHolder.Instance = container;
        Assert.That(UnityContainerHolder.Instance, Is.SameAs(container));
      });
    }

    [Test]
    public void Instance_returns_the_same_instance_every_time()
    {
      var c1 = UnityContainerHolder.Instance;
      var c2 = UnityContainerHolder.Instance;
      Assert.That(c1, Is.SameAs(c2));
    }

    private void WithInstanceFactoryRestored(Action action)
    {
      var originalInstanceFactory = UnityContainerHolder.InstanceFactory;
      try
      {
        action.Invoke();
      }
      catch (Exception)
      {
        UnityContainerHolder.InstanceFactory = originalInstanceFactory;
        throw;
      }
    }
    
    private void WithInstanceRestored(Action action)
    {
      var originalInstance = UnityContainerHolder.Instance;
      try
      {
        action.Invoke();
      }
      catch (Exception)
      {
        UnityContainerHolder.Instance = originalInstance;
        throw;
      }
    }
  }


  [TestFixture]
  public class UnityInjectionFactoryTests
  {
    public class SampleDependency{}

    [Test]
    public void Ctor_initializes_the_UnityContainerInitializer()
    {
      WithRestoreInitializeAction(() =>
      {
        var called = 0;
        Action initializer = () => called++;
        UnityContainerInitializer.InitializeAction = initializer;
        var factory = new UnityInjectionFactory();
        Assert.That(called, Is.EqualTo(1));
      });
    }

    [Test]
    public void Create_uses_instances_Resolve_method_to_retrieve_type()
    {
      WithRestoreInitializeAction(() =>
      {
        WithInstanceRestored(() =>
        {
          var unityContainer = new UnityContainer();
          UnityContainerInitializer.InitializeAction = () => { };
          UnityContainerHolder.Instance = unityContainer;
          var factory = new UnityInjectionFactory();
          var dep = factory.Create(typeof(SampleDependency));
          Assert.That(dep, Is.Not.Null);
          Assert.That(dep, Is.InstanceOf<SampleDependency>());
        });
      });
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

    private void WithInstanceRestored(Action action)
    {
      var originalInstance = UnityContainerHolder.Instance;
      try
      {
        action.Invoke();
      }
      catch (Exception)
      {
        UnityContainerHolder.Instance = originalInstance;
        throw;
      }
    }



  }
}