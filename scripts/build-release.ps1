<#
.SYNOPSIS
    Local build script for SkylineWeather – mirrors the GitHub Actions release-builder workflow.

.DESCRIPTION
    Builds the FluentWeather UWP project for the specified architectures using MSBuild.
    Output packages are placed under <SolutionRoot>\Package\<arch>\.
    This script does NOT create a GitHub Release or upload assets.

.PARAMETER Version
    The package version to embed in Package.appxmanifest (e.g. 1.2.3.4).
    If omitted the current value in the manifest is kept unchanged.

.PARAMETER Architectures
    Comma-separated list of architectures to build.
    Allowed values: x86, x64, ARM64
    Defaults to all three: x86, x64, ARM64

.EXAMPLE
    # Build all architectures with version 1.0.0.0
    .\scripts\build-release.ps1 -Version 1.0.0.0

.EXAMPLE
    # Build only x64 and ARM64
    .\scripts\build-release.ps1 -Version 2.0.0.1 -Architectures x64,ARM64

.EXAMPLE
    # Build all architectures without changing the version
    .\scripts\build-release.ps1
#>

[CmdletBinding()]
param(
    [string]$Version,

    [ValidateSet('x86', 'x64', 'ARM64')]
    [string[]]$Architectures = @('x86', 'x64', 'ARM64')
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# ---------------------------------------------------------------------------
# Resolve paths relative to the repository root (the parent of this script's
# directory so the script can be invoked from any working directory).
# ---------------------------------------------------------------------------
$ScriptDir    = Split-Path -Parent $MyInvocation.MyCommand.Definition
$RepoRoot     = Split-Path -Parent $ScriptDir
$SolutionPath = Join-Path $RepoRoot 'FluentWeather.sln'
$ManifestPath = Join-Path $RepoRoot 'FluentWeather.Uwp\Package.appxmanifest'
$PackageRoot  = Join-Path $RepoRoot 'Package'

# ---------------------------------------------------------------------------
# Validate required files
# ---------------------------------------------------------------------------
if (-not (Test-Path $SolutionPath)) {
    Write-Error "Solution file not found: $SolutionPath"
    exit 1
}
if (-not (Test-Path $ManifestPath)) {
    Write-Error "App manifest not found: $ManifestPath"
    exit 1
}

# ---------------------------------------------------------------------------
# Locate MSBuild
# ---------------------------------------------------------------------------
$msbuild = Get-Command msbuild -ErrorAction SilentlyContinue

if (-not $msbuild) {
    # Try the default Visual Studio 2022 install locations
    $vsPaths = @(
        'C:\Program Files\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\MSBuild.exe',
        'C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe',
        'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe',
        'C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin\MSBuild.exe',
        'C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe',
        'C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe'
    )
    foreach ($path in $vsPaths) {
        if (Test-Path $path) {
            $msbuild = $path
            break
        }
    }
}

if (-not $msbuild) {
    Write-Error @"
MSBuild was not found on this machine.
Please install Visual Studio 2022 (or 2019) with the 'Universal Windows Platform development' workload,
or add MSBuild to your PATH (e.g. via 'Developer Command Prompt for VS').
"@
    exit 1
}

$MSBuildExe = if ($msbuild -is [System.Management.Automation.CommandInfo]) { $msbuild.Source } else { $msbuild }
Write-Host "Using MSBuild: $MSBuildExe" -ForegroundColor Cyan

# ---------------------------------------------------------------------------
# Update manifest version (optional)
# ---------------------------------------------------------------------------
if ($Version) {
    Write-Host "Updating manifest version to $Version ..." -ForegroundColor Cyan
    [xml]$manifest = Get-Content $ManifestPath
    $manifest.Package.Identity.Version = $Version
    $manifest.Save($ManifestPath)
    Write-Host "Manifest updated." -ForegroundColor Green
} else {
    [xml]$manifest = Get-Content $ManifestPath
    $Version = $manifest.Package.Identity.Version
    Write-Host "No -Version supplied; using existing manifest version: $Version" -ForegroundColor Yellow
}

# ---------------------------------------------------------------------------
# Build each requested architecture
# ---------------------------------------------------------------------------
$failed = @()

foreach ($arch in $Architectures) {
    $outputDir = Join-Path $PackageRoot $arch

    # Ensure output directory exists
    if (-not (Test-Path $outputDir)) {
        New-Item -ItemType Directory -Path $outputDir -Force | Out-Null
    }

    Write-Host ""
    Write-Host "========================================" -ForegroundColor Cyan
    Write-Host " Building $arch ..." -ForegroundColor Cyan
    Write-Host "========================================" -ForegroundColor Cyan

    $msbuildArgs = @(
        $SolutionPath,
        "/p:Platform=$arch",
        "/p:AppxBundlePlatforms=$arch",
        "/p:AppxPackageDir=$outputDir\",
        '/p:AppxPackageSigningEnabled=false',
        '/p:Configuration=Release',
        '/p:BuildMode=StoreUpload',
        '/restore'
    )

    & $MSBuildExe @msbuildArgs

    if ($LASTEXITCODE -ne 0) {
        Write-Warning "Build FAILED for $arch (exit code $LASTEXITCODE)."
        $failed += $arch
    } else {
        Write-Host "Build SUCCEEDED for $arch. Output: $outputDir" -ForegroundColor Green
    }
}

# ---------------------------------------------------------------------------
# Summary
# ---------------------------------------------------------------------------
Write-Host ""
Write-Host "========================================"
if ($failed.Count -eq 0) {
    Write-Host "All builds completed successfully." -ForegroundColor Green
    Write-Host "Packages are in: $PackageRoot"
} else {
    Write-Warning "The following architectures failed to build: $($failed -join ', ')"
    exit 1
}
