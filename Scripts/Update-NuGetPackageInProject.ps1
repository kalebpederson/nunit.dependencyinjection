param(
  [Parameter(Mandatory=$true)]
  [string]$source,                # nuget package source
  [Parameter(Mandatory=$true)]
  [string]$id,                    # nuget package id
  [Parameter(Mandatory=$true)]
  [string]$installProject         # the project to install into
)

if (-not (test-path -type Leaf $installProject)) {
  write-error 'installproject must be a valid file'
  exit 1
}

if (-not (test-path -type Container $source)) {
  write-error 'source must be a valid directory'
  exit 2
}

$sourceDir = (resolve-path $source).Path
$projectPath = @(dir $installProject)
pushd $projectPath.Directory.FullName > $null 2>&1
try
{
  write-host "Installing package $id from source $sourceDir into '$($projectPath.Name)' ..."
  dotnet add $projectPath.FullName package -s $sourceDir $id
  $completedSuccessfully = ($? -and  $LASTEXITCODE -eq 0)
  if (-not $completedSuccessfully)
  {
    exit 3
  }
}
finally
{
  popd
}
