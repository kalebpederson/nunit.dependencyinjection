#!/usr/bin/env pwsh
dotnet test Main/NUnit.Extension.DependencyInjection.sln
if ($LASTEXITCODE -ne 0) {
  Write-Error "dotnet test Main/NUnit.Extensions.DependencyInjection.sln step failed"
  exit 1
}
dotnet pack --no-build Main/NUnit.Extension.DependencyInjection.sln
if ($LASTEXITCODE -ne 0) {
  Write-Error "dotnet pack step failed"
  exit 2
}
./Scripts/Update-ValidationSolutionNuGetPackages.ps1 -basedir .
if ($LASTEXITCODE -ne 0) {
  Write-Error "Scripts/Update-ValidationSolutionNuGetPackages failed"
  exit 3
}
dotnet test Validation/ValidationTests.sln
if ($LASTEXITCODE -ne 0) {
  Write-Error "dotnet test Validation/ValidationTests.sln step failed"
  exit 4
}
Write-Host "Build completed successfully."
