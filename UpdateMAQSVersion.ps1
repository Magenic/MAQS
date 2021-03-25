<#
.SYNOPSIS
    Makes updates to the MAQS project templates.
.DESCRIPTION
    This powershell script is used to update the MAQS csproj files to specific versions
.PARAMETER MAQSVersion
    The desired version of MAQS to set.
.PARAMETER BetaVersion
    The desired version of the beta release to set.
.NOTES
  Version:        1.0
  Author:         Magenic
  Creation Date:  03/24/2021
  Purpose/Change: Initial script development. 
.EXAMPLE
  ./UpdateMAQSVersion -MAQSVersion "4.0.0" -BetaVersion "-beta.1"

  This command will update the MAQS version to the specific version with the next beta tag.
#>

[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [string]$MAQSVersion,
    [string]$BetaVersion
)

function UpdateFiles($directory, $fileFilter, $maqsVersion, $BetaVersion) {
    Get-ChildItem $directory -Filter $fileFilter -Recurse |
    ForEach-Object {
        Write-Host "Checking File " $_.FullName
        UpdateFileContent $_.FullName $maqsVersion $BetaVersion
    }
}

function UpdateFileContent($file, $maqsVersion, $betaVersion) {
    $filetext = [System.IO.File]::ReadAllText($file)
    $filetext = UpdateLine $filetext $maqsVersion $betaVersion
    [System.IO.File]::WriteAllText($file, $filetext, [System.Text.Encoding]::UTF8)
}

function UpdateLine($fileText, $maqsVersion, $betaVersion) {
    $originalText = $filetext
    $regexPattern = "(<BuildVersion>)([\d\.]*)(</BuildVersion>)"
        
    $maqsVersion = "`${1}" + $maqsVersion + "`${3}"
    $filetext = $filetext -replace $regexPattern, $maqsVersion
        
    $regexPattern = "(<PreRelease>)([\d\.]*)(</PreRelease>)"
    $betaVersion = "`${1}" + $betaVersion + "`${3}"
    $filetext = $filetext -replace $regexPattern, $betaVersion
       
    if($originalText -eq $filetext){
        Write-Host "No Changes to file"
    }
    else{
        Write-Host "File Updated"
    }
    
    return $filetext
}

UpdateFiles $PSScriptRoot "*.csproj" $MAQSVersion $BetaVersion