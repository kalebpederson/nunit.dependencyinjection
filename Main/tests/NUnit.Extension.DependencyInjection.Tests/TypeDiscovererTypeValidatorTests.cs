// Copyright (c) Kaleb Pederson Software LLC. All rights reserved.
// Licensed under the MIT license. See LICENSE file alongside the solution file for full license information.

using System;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Extension.DependencyInjection.Abstractions;
using NUnit.Framework;

namespace NUnit.Extension.DependencyInjection.Tests
{
  [TestFixture]
  public class TypeDiscovererTypeValidatorTests
  {
    [Test]
    public void AssertIsValidDiscovererType_throws_ArgumentNullException_when_provided_info_is_null()
    {
      Assert.That(
        () => TypeDiscovererTypeValidator.AssertIsValidDiscovererType(null),
        Throws.ArgumentNullException
          .With.Message.Match("info must be non-null\\.")
        );
    }
    
    [Test]
    public void AssertIsValidDiscovererType_throws_ArgumentNullException_when_provided_discoverer_type_is_null()
    {
      var discovererTypeInfo = new TypeDiscovererInfo
      {
        DiscovererType = null,
        DiscovererArgumentTypes = new Type[] {},
        DiscovererArguments = new object[] {}
      };
      Assert.That(
        () => TypeDiscovererTypeValidator.AssertIsValidDiscovererType(discovererTypeInfo),
        Throws.ArgumentNullException
          .With.Message.Match("specified as ITypeDiscoverer .* cannot be null")
        );
    }

    [TestCase(typeof(int))]
    [TestCase(typeof(DateTime))]
    public void AssertIsValidDiscovererType_throws_ArgumentOutOfRangeException_when_provided_type_is_not_an_ITypeDiscoverer(Type type)
    {
      var discovererTypeInfo = new TypeDiscovererInfo
      {
        DiscovererType = type,
        DiscovererArgumentTypes = new Type[] {},
        DiscovererArguments = new object[] {}
      };
      Assert.That(
        () => TypeDiscovererTypeValidator.AssertIsValidDiscovererType(discovererTypeInfo),
        Throws.Exception.TypeOf<ArgumentOutOfRangeException>()
          .With.Message.Match("must be of type ITypeDiscoverer")
        );
    }
    
    [TestCase(typeof(NoArgsTypeDiscoverer), new object[] {32})]
    public void AssertIsValidDiscovererType_throws_ArgumentOutOfRangeException_when_no_matching_noargs_ctor_is_found(Type type, object[] args)
    {
      var discovererTypeInfo = new TypeDiscovererInfo
      {
        DiscovererType = type,
        DiscovererArgumentTypes = args.Select(x => x.GetType()).ToArray(),
        DiscovererArguments = args
      };
      Assert.That(
        () => TypeDiscovererTypeValidator.AssertIsValidDiscovererType(discovererTypeInfo),
        Throws.Exception.TypeOf<ArgumentOutOfRangeException>()
          .With.Message.Match(
            Regex.Escape($"{type.FullName}") + ".* must have a constructor which corresponds" +
            " to the argument types which were provided"
            )
        );
    }
    
    [TestCase(typeof(NoArgsTypeDiscoverer), new object[0])]
    [TestCase(typeof(IntConstructedTypeDiscoverer), new object[] {42})]
    public void AssertIsValidDiscovererType_throws_nothing_when_type_and_arguments_match_ITypeDiscoverer(Type type, object[] args)
    {
      var discovererTypeInfo = new TypeDiscovererInfo
      {
        DiscovererType = type,
        DiscovererArgumentTypes = args.Select(x => x.GetType()).ToArray(),
        DiscovererArguments = args
      };
      Assert.That(
        () => TypeDiscovererTypeValidator.AssertIsValidDiscovererType(discovererTypeInfo),
        Throws.Nothing);
    }
  
    private class NoArgsTypeDiscoverer : ITypeDiscoverer 
    {
      /// <inheritdoc />
      public void Discover(object container)
      {
      }
    }
    
    private class IntConstructedTypeDiscoverer : ITypeDiscoverer
    {
      public IntConstructedTypeDiscoverer(int _)
      {
      }
      
      /// <inheritdoc />
      public void Discover(object container)
      {
      }
    }
    
  }
}