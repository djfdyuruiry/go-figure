param (
  $Version = "DEFAULT"
)

$repoPath = Split-Path -Parent $PSScriptRoot

$assemblyPath = Join-Path $repoPath "GoFigure.App/Properties/AssemblyInfo.cs"

function Main() {
  if ($Version -eq "DEFAULT") {
    Write-Host "No version specified, default version will be used"
    return
  }

  $VersionWithoutPrefix = $Version.Replace("v", "")
  $Version = "${VersionWithoutPrefix}.0.0"

  Write-Host "Setting 'Go Figure!' version - Version=${Version}"

  (Get-Content -Path $assemblyPath) -Replace "0.0.0.0", $Version | `
    Set-Content -Path $assemblyPath

  Write-Host "Version has been set"
}

Main
