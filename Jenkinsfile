properties([
    parameters([
        string (name: 'branchName', defaultValue: 'master', description: 'Branch to get the tests from')
    ])
])

def isFailed = false
def branch = params.branchName
def buildArtifactsFolder = "C:/BuildPackagesFromPipeline/$BUILD_ID"
currentBuild.description = "Branch: $branch"

def RunNUnitTests(String pathToDll, String condition, String reportName)
{
    try
    {
        bat & "C:\\Program Files (x86)\\NUnit.org\\nunit-console\\nunit3-console.exe $pathToDll $condition --result=$reportName"
    }
    finally
    {
        stash name: reportName, includes: reportName
    }
}

node('master') 
{
    stage('Checkout')
    {
        git branch: branch, url: 'https://github.com/andrii-solaris/atata-phptravels-uitests.git'
    }
    
    stage('Restore NuGet')
    {
        powershell ".\\build.ps1 RestoreNuGetPackages"
    }

    stage('Build Solution')
    {
        powershell ".\\build.ps1 BuildSolution"
    }

    stage('Copy Artifacts')
    {
        powershell ".\\build.ps1 CopyArtifacts -BuildArtifactsFolder $buildArtifactsFolder"
    }
}

catchError
{
    isFailed = true
    stage('Run Tests')
    {
        parallel FirstTest: {
            node('master') {
                RunNUnitTests("$buildArtifactsFolder/Debug/PhpTravels.UITests.dll", "--where cat==FirstTest", "TestResult1.xml")
            }
        }, SecondTest: {
            node('Slave') {
                RunNUnitTests("$buildArtifactsFolder/Debug/PhpTravels.UITests.dll", "--where cat==SecondTest", "TestResult2.xml")
            }
        }
    }
    isFailed = false
}

node('master')
{
    stage('Reporting')
    {
        unstash "TestResult1.xml"
        unstash "TestResult2.xml"

        archiveArtifacts '*.xml'
        nunit testResultsPattern: 'TestResult1.xml, TestResult2.xml'

        if(isFailed)
        {
            slackSend color: 'danger', message: 'Tests failed.'
        }
        else
        {
            slackSend color: 'good', message: 'Tests passed.'
        }
    }
}