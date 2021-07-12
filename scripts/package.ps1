param (
  $Version = "GIT_COMMIT"
)

$repoPath = Split-Path -Parent $PSScriptRoot

$buildPath = Join-Path $repoPath "GoFigure.App\bin\Release"

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

  Remove-Build-Items "*.pdb"
  Remove-Build-Items "*.zip"
  Remove-Build-Items "*.xml"
  Remove-Build-Items "*.exe.config"

  $outputPath = Join-Path $buildPath "go-figure-${Version}.zip"
  
  Write-Host "Building zip package @ $outputPath"

  Compress-Archive "${buildPath}/*" -DestinationPath $outputPath

  Write-Host "Packaging complete"
}

Main
