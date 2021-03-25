<#
.SYNOPSIS
    Makes updates to the MAQS project templates.
.DESCRIPTION
    This powershell script is used to update the MAQS project templates. It updates the specified references in the unzipped templates. Editing of parameters in the powershell file will be necessary to set the desired packages and related versions to update.
.PARAMETER MAQSVersion
    The desired version of MAQS to set.
.PARAMETER BetaVersion
    Is a beta release
.NOTES
  Version:        1.0
  Author:         Magenic
  Creation Date:  03/24/2021
  Purpose/Change: Initial script development. 
.EXAMPLE
  ./UpdateMAQSVersion

  This command will update the MAQS version to the next minor increment.
.EXAMPLE
  ./UpdateMAQSVersion -BetaVersion $true

  This command will update the MAQS version to the next beta version.
.EXAMPLE
  ./UpdateMAQSVersion -MAQSVersion "4.0.0"

  This command will update the MAQS version to the specific version.

.EXAMPLE
  ./UpdateMAQSVersion -MAQSVersion "4.0.0" -BetaVersion $true

  This command will update the MAQS version to the specific version with the next beta tag.
#>

[CmdletBinding()]
param(
    [Parameter(Mandatory = $false)]
    [string]$MAQSVersion = "6.2.3",
    [string]$BetaVersion = "-beta.1"
)


#function UpdateFiles($directory, $fileFilter, $regexType, $matchValueList, $replaceValueList){
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

# [regex]::match($temp,'(?<=<PreRelease>).*(?=</PreRelease>)')

UpdateFiles $PSScriptRoot "*.csproj" $MAQSVersion $BetaVersion






