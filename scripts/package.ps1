param (
  $Version = "GIT_COMMIT"
)

$repoPath = Split-Path -Parent $PSScriptRoot

$buildPath = Join-Path $repoPath "GoFigure.App/bin/Release/net5.0-windows/win-x64/publish"

function Remove-Build-Items() {
  param ($Filter)

  Get-ChildItem $buildPath -Filter $Filter | `
    ForEach-Object { Remove-Item $_.FullName -Force  }
}

function Main() {
  if ($env:GITHUB_REF -and $env:GITHUB_REF.StartsWith("v")) {
    $Version = $env:GITHUB_REF
  }

  if ($Version -eq "GIT_COMMIT") {
    $Version = git rev-parse --short HEAD
  }

  Write-Host "Packaging 'Go Figure!' - Version=${Version}"

  Write-Host "Cleaning build output"

  Remove-Build-Items "*.dll.config"
  Remove-Build-Items "*.zip"

  $outputZip = "go-figure-${Version}.zip"
  $outputPath = Join-Path $buildPath $outputZip
  
  Write-Host "Building zip package: $outputZip"

  Compress-Archive "${buildPath}/*" -DestinationPath $outputPath

  Write-Host "Packaging complete"

  if ($env:GITHUB_ENV) {
    Add-Content -Path $env:GITHUB_ENV -Value "APP_PACKAGE_NAME=$outputZip"
  }
}

Main
