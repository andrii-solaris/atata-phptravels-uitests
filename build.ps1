#Requires -Version 5.0

param
(
    [Parameter()]
    [String[]] $TaskList = @("RestorePackages", "Build", "CopyArtifacts"),
    
    # Also add following parameters: 
    #   Configuration
    #   Platform
    #   OutputPath
    # And use these parameters inside BuildSolution function while calling for MSBuild.
    # Use /p swith to pass the parameter. For example:
    #   MSBuild.exe src/Solution.sln /p:Configuration=$Configuration
    # More info here: https://docs.microsoft.com/en-us/visualstudio/msbuild/common-msbuild-project-properties?view=vs-2017

    [Parameter()]
    [String] $BuildArtifactsFolder,

    [Parameter()]
    [String] $Configuration = "Debug",

    [Parameter()]
    [String] $Platform = "Any CPU",

    [Parameter()]
    [String] $OutputPath ="src\PhpTravels.UITests\bin\Debug",

    [Parameter()] 
    [String] $Solution = "src\PhpTravels.UITests.sln"
)

$NugetUrl = "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe"
$NugetExe = Join-Path $PSScriptRoot "nuget.exe"
$MSBuildExe = "C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\MSBuild.exe"
$NUnitConsoleExe = "C:\Program Files (x86)\NUnit.org\nunit-console\nunit3-console.exe"
# Define additional variables here (MSBuild path, etc.)

Function DownloadNuGet()
{
    if (-Not (Test-Path $NugetExe)) 
    {
        Write-Output "Installing NuGet from $NugetUrl..."
        Invoke-WebRequest $NugetUrl -OutFile $NugetExe -ErrorAction Stop
    }
}

Function RestoreNuGetPackages()
{
    DownloadNuGet
    Write-Output 'Restoring NuGet packages...'

    # NuGet.exe call here
    & $NugetExe restore $Solution

    if($LASTEXITCODE -ne 0){
        Throw "An error occured while restoring nuget packages."
    }
}

Function BuildSolution()
{
    Write-Output "Building '$Solution' solution..."

    # MSBuild.exe call here
    & $MSBuildExe $Solution /p:Configuration=$Configuration /p:Platform=$Platform
     if($LASTEXITCODE -ne 0){
        Throw "An error occured while building solution."
    }
}

Function CopyBuildArtifacts()
{
    param
    (
        [Parameter(Mandatory)]
        [String] $SourceFolder,
        [Parameter(Mandatory)]
        [String] $DestinationFolder
    )

    # Copy all files from $SourceFolder to $DestinationFolder
    #
    # Useful commands:
    #   Test-Path - check if path exists
    #   Remove-Item - remove folders/files
    #   New-Item - create folder/file
    #   Get-ChildItem - gets items from specified path
    #   Copy-Item - copies item into destination folder
    #
    #     NOTE: you can chain methods using pipe (|) symbol. For example:
    #           Get-ChildItem ... | Copy-Item ...
    #
    #           which will get items (Get-ChildItem) and will copy them (Copy-Item) to the target folder
   
    if (Test-Path -Path $DestinationFolder)
    {
        Remove-Item -Path $DestinationFolder -Recurse
    }

    New-Item -Path $DestinationFolder -ItemType "directory"

    Copy-item -Force -Recurse -Verbose $SourceFolder -Destination $DestinationFolder
}

foreach ($Task in $TaskList) {
    if ($Task.ToLower() -eq 'restorepackages')
    {
        RestoreNuGetPackages
    }
    if ($Task.ToLower() -eq 'build')
    {
        BuildSolution
    }
    if ($Task.ToLower() -eq 'copyartifacts')
    {
        CopyBuildArtifacts "$OutputPath" "$BuildArtifactsFolder"
    }
}