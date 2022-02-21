[CmdletBinding()]
Param(
    [string]$Script = "build.cake",
    [string]$Target = "Default",
    [ValidateSet("Release", "Debug")]
    [string]$Configuration = "Release",
    [ValidateSet("Quiet", "Minimal", "Normal", "Verbose", "Diagnostic")]
    [string]$Verbosity = "Verbose",
    [Alias("DryRun","Noop")]
    [switch]$WhatIf,
    [Parameter(Position=0,Mandatory=$false,ValueFromRemainingArguments=$true)]
    [string[]]$ScriptArgs
)

Write-Host "Preparing to run build script..."

if(!$PSScriptRoot){
    $PSScriptRoot = Split-Path $MyInvocation.MyCommand.Path -Parent
}

$TOOLS_DIR = Join-Path $PSScriptRoot "tools"
$TEMP_DIR = Join-Path $PSScriptRoot "tmp"
$TEMP_PROJECT = Join-Path $TEMP_DIR "tmp.csproj"
$CLI_DIR = Join-Path $PSScriptRoot "cli"
$DOTNET_PATH = Join-Path $CLI_DIR "dotnet.exe"

# Is this a dry run?
$UseDryRun = "";
if($WhatIf.IsPresent) 
{
    $UseDryRun = "--dryrun"
}

& dotnet new classlib -o "$TEMP_DIR" --no-restore
& dotnet add "$TEMP_PROJECT" package --package-directory "$TOOLS_DIR" Cake.CoreCLR
Remove-Item -Recurse -Force "$TEMP_DIR"
$CakePath = Get-ChildItem -Filter Cake.dll -Recurse | Sort-Object -Descending | Select-Object -Expand FullName -first 1

if (!(Test-Path $DOTNET_PATH)) 
{
	Write-Host "Downloading runtime..."
	powershell -NoProfile -ExecutionPolicy unrestricted -Command "&([scriptblock]::Create((Invoke-WebRequest -useb 'https://dot.net/v1/dotnet-install.ps1'))) -Version 6.0.2 -SharedRuntime -InstallDir $CLI_DIR -NoPath"
}

Write-Host "Running build script..."
& "$DOTNET_PATH" "$CakePath" $Script --nuget_useinprocessclient=true --target=$Target --configuration=$Configuration --verbosity=$Verbosity $UseDryRun $ScriptArgs
exit $LASTEXITCODE