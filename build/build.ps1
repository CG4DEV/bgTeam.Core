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
	powershell -NoProfile -ExecutionPolicy unrestricted -Command "&([scriptblock]::Create((Invoke-WebRequest -useb 'https://dot.net/v1/dotnet-install.ps1'))) -Version 2.0.0 -SharedRuntime -InstallDir $CLI_DIR -NoPath"
}

&{
    &{
        #Put install into image???
        dotnet tool install -g dotnet-reportgenerator-globaltool
    }
    #@DevOps, Insert here path to repository
    $RepositoryDirectory = "../"
    #@DevOps, Insert here path to artefacts (for report)
    $ArtefactsDirectory = "../"
    &{
        #Generate coverlet reports
        dotnet test $RepositoryDirectory --collect:"XPlat Code Coverage"
    }
    &{
        #Find coverage reports and generate html summary
        $outPutDirectory = $ArtefactsDirectory + "coverage"
        $reportFileName = "coverage.cobertura.xml"
        $_reportGeneratorInpuString = ""
        Get-ChildItem -Path $RepositoryDirectory -Filter $reportFileName -Recurse | ForEach-Object {
            $_reportGeneratorInpuString += $_.FullName + ";"
        }
        reportgenerator "-reports:$_reportGeneratorInpuString" "-targetdir:$outPutDirectory" -reporttypes:Html
    }
    &{
        #Make coverage badge
        $coverageReportPath = $ArtefactsDirectory + "coverage/index.html"
        $outBadgeFileName = $ArtefactsDirectory + "coverage/coveradge-badge.svg"

        $coverageBadgePath = $RepositoryDirectory + "static-templates/coveradge-badge-template.svg"
        $coverageLinePattern = "<tr><th>Line coverage:</th><td>\d+.\d+"
        $templateMark = "@CoverageNumb"
        $val = Get-Content $coverageReportPath | Select-String -Pattern $coverageLinePattern | ForEach-Object { $_ -match "(\d+.\d+)" } | ForEach-Object { ([decimal]$Matches[0]) }
        $CoverageBadgeContent = Get-Content $coverageBadgePath
        $CoverageBadgeContent.replace($templateMark, $val) | Out-File $outBadgeFileName
    }
}

Write-Host "Running build script..."
& dotnet "$CakePath" $Script --nuget_useinprocessclient=true --target=$Target --configuration=$Configuration --verbosity=$Verbosity $UseDryRun $ScriptArgs 
exit $LASTEXITCODE