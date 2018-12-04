# Generates NuGet packages from the main code, then validates that the
# validation tests can run against the generated NuGet packages.
param([string]$basedir, [string]$configuration="debug")

$validationDir = (join-path $basedir -ChildPath "Validation")
$mainDir = (join-path $basedir -ChildPath "Main")
$solutionFile = (join-path $mainDir -ChildPath "NUnit.Extension.DependencyInjection.sln")
$validationSolution = (join-path $validationDir -ChildPath "NUnit.Extension.DependencyInjection.ValidationTests.sln")
$validationTestDir = (join-path $validationDir -ChildPath "NUnit.Extension.DependencyInjection.Unity.IntegrationTests")
$dependencyInjectionProjDir = (join-path $mainDir -ChildPath "NUnit.Extension.DependencyInjection")
$dependencyInjectionUnityProjDir = (join-path $mainDir -ChildPath "NUnit.Extension.DependencyInjection.Unity")
$dependencyInjectionNupkgDir = (join-path $dependencyInjectionProjDir -ChildPath "bin\$configuration")
$dependencyInjectionUnityNupkgDir = (join-path $dependencyInjectionUnityProjDir -ChildPath "bin\$configuration")

# build the NuGet packages to use as the source files
dotnet pack $solutionFile

# update the existing project to use the just-built NuGet packages
try
{
  pushd $validationTestDir > $null 2>&1
  dotnet add package NUnit.Extension.DependencyInjection -s $dependencyInjectionNupkgDir
  dotnet add package NUnit.Extension.DependencyInjection.Unity -s $dependencyInjectionUnityNupkgDir
}
finally
{
  popd > $null 2>&1
}

try
{
  pushd $validationDir > $null 2>&1
  dotnet test
}
finally
{
  popd > $null 2>&1
}

