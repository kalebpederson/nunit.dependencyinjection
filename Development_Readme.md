# Introduction 

The root of this project contains three different folders:
* Main - the source code for the NuGet packages
* Scripts - scripts used for building and validating the generated code 
  and NuGet packages.
* Validation - source code used to validate that the generated NuGet
  packages work against prior versions of the API.
  
 # Building the Source
 
 Build the application per the following steps:
 
 1. dotnet test Main\NUnit.Extension.DependencyInjection.sln
 1. dotnet pack --no-build Main\NUnit.Extension.DependencyInjection.sln
 
 Step 1 above will cause a restore of NuGet packages to take place, a 
 build of the solution, followed by the execution of the unit tests.
 
 Step 2 builds the NuGet packages that will be consumed in the validation
 steps.

# Validating Changes
 
 First and foremost, write unit tests that provide good coverage of the
 code and/or changes.
 
 Now, assume for a moment that the code is perfect. Even with perfect
 code we still haven't validated that the NuGet package is valid nor have 
 we validated that the changes are backwards compatible (i.e., don't break 
 existing consumers).
 
 To perform this validation we have a second solution that validates tests
 that perform dependency injection can be properly run and executed.
 
 To perform build validation perform the following steps:
 
 1. . .\scripts\Update-ValidationSolutionNuGetPackages.ps1 -basedir .
 1. dotnet test Validation\ValidationTests.sln
 
 The first step updates the NuGet package reference in the validation 
 solution, ensuring that it points at the just-packed NuGet package.
 
 The second step runs the tests using the new NuGet packages as the
 dependency injection framework.
