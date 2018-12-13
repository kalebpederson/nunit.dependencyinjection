using System;

namespace NUnit.Extension.DependencyInjection
{
  /// <summary>
  /// Attribute whose presence within an assembly indicates that the
  /// assembly should be scanned when types are being discovered
  /// by the <see cref="ITypeDiscoverer"/> and said type discoverer
  /// is of the type that does convention based assembly scanning.
  /// </summary>
  [AttributeUsage(AttributeTargets.Assembly)]
  public class NUnitAutoScanAssemblyAttribute : Attribute
  {
  }
}