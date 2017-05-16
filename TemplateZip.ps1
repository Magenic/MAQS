<#
.SYNOPSIS
    Zips the MAQS project templates.
.DESCRIPTION
    This powershell script is used to zip the MAQS project templates. Editing of parameters in the powershell file may be necessary.
.PARAMETER closedSource
    Set false if the closeSource version of MAQS shouldn't be updated
.PARAMETER openSource
    Set true if the openSource version of MAQS should be updated
.NOTES
  Version:        1.0
  Author:         Magenic
  Creation Date:  05/16/2017
  Purpose/Change: Initial script development. 
  
.EXAMPLE
  ./TemplateUpdates

  This command will zip to the open or closed source version of MAQs, depending on which flags are hardcoded to default to true.
.EXAMPLE
  ./TemplateUpdates -openSource $true

  This command will zip the open source templates.
#>

param (
    [bool]$closedSource = $true,
    [bool]$openSource = $false
)

###################################################################################################################
function ZipFiles($inputDirectory, $outputDirectory){
    $nunitDir1 = $inputDirectory + "\NUnit"
    $nunitDir2 = $inputDirectory + "\NUnit Only"

    Set-Location $inputDirectory
    $relativePath = Get-ChildItem $inputDirectory -Directory | Resolve-Path -Relative
    ForEach($file in $relativePath){
        $file = $file.TrimStart(".", " ", "\")
        $input = $inputDirectory + "\" + $file
        $inputDir = $input + "\*"
        $destination = $outputDirectory + "\" + $file + ".zip"

        if(($input -ne $nunitDir1) -and ($input -ne $nunitDir2) -and ($input -ne $outputDirectory)){
            Write-Host "Zipping " $input
            Compress-Archive -Path $inputDir -DestinationPath $destination -Force
        }
    }
}

function WorkflowFunction($closedSource, $openSource){
    if($closedSource){
        ZipFiles $PSScriptRoot"\Extensions\VisualStudioQatExtension\ProjectTemplates\Magenic Test" $PSScriptRoot"\Extensions\VisualStudioQatExtension\ProjectTemplates\Magenic Test"
        ZipFiles $PSScriptRoot"\Extensions\VisualStudioQatExtension\ProjectTemplates\Magenic Test\NUnit" $PSScriptRoot"\Extensions\VisualStudioQatExtension\ProjectTemplates\Magenic Test\Nunit"
    }
    if($openSource){
        ZipFiles $PSScriptRoot"\Extensions\VisualStudioQatExtensionOss\ProjectTemplates\Magenic's Open Test" $PSScriptRoot"\Extensions\VisualStudioQatExtensionOss\ProjectTemplates\Magenic's Open Test"
        ZipFiles $PSScriptRoot"\Extensions\VisualStudioQatExtensionOss\ProjectTemplates\Magenic's Open Test\NUnit Only" $PSScriptRoot"\Extensions\VisualStudioQatExtensionOss\ProjectTemplates\Magenic's Open Test\NUnit Only"
    }
}

WorkflowFunction $closedSource $openSource