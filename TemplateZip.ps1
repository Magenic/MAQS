<#
.SYNOPSIS
    Zips the MAQS project templates.
.DESCRIPTION
    This powershell script is used to zip the MAQS project templates. Editing of parameters in the powershell file may be necessary.
.PARAMETER closedSource
    Set false if the closeSource version of MAQS shouldn't be updated
.PARAMETER openSource
    Set true if the openSource version of MAQS should be updated
.PARAMETER specSource
    Set true if the SpecFlow version of MAQS should be updated
.NOTES
  Version:        1.0
  Author:         Magenic
  Creation Date:  05/16/2017
  Purpose/Change: Initial script development. 

  Version:        2.0
  Author:         Magenic
  Creation Date:  07/9/2018
  Purpose/Change: Add SpecFlow build support. Add item template support.
  
.EXAMPLE
  ./TemplateUpdates

  This command will zip to the open or closed source version of MAQs, depending on which flags are hardcoded to default to true.
.EXAMPLE
  ./TemplateUpdates -openSource $true

  This command will zip the open source templates.
#>

param (
    [bool]$closedSource = $true,
    [bool]$openSource = $true,
    [bool]$specSource = $true
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

function WorkflowFunction($closedSource, $openSource, $specSource){
    if($closedSource){
        ZipFiles $PSScriptRoot"\Extensions\VisualStudioQatExtension\ProjectTemplates\Magenic Test" $PSScriptRoot"\Extensions\VisualStudioQatExtension\ProjectTemplates\Magenic Test"
        ZipFiles $PSScriptRoot"\Extensions\VisualStudioQatExtension\ProjectTemplates\Magenic Test Core" $PSScriptRoot"\Extensions\VisualStudioQatExtension\ProjectTemplates\Magenic Test Core"
        ZipFiles $PSScriptRoot"\Extensions\VisualStudioQatExtension\ItemTemplates\Magenic Test" $PSScriptRoot"\Extensions\VisualStudioQatExtension\ItemTemplates\Magenic Test"
    }
    if($openSource){
        ZipFiles $PSScriptRoot"\Extensions\VisualStudioQatExtensionOss\ProjectTemplates\Magenic's Open Test" $PSScriptRoot"\Extensions\VisualStudioQatExtensionOss\ProjectTemplates\Magenic's Open Test"
        ZipFiles $PSScriptRoot"\Extensions\VisualStudioQatExtensionOss\ItemTemplates\Magenic's Open Test" $PSScriptRoot"\Extensions\VisualStudioQatExtensionOss\ItemTemplates\Magenic's Open Test"
    }
    if($specSource){
        ZipFiles $PSScriptRoot"\Extensions\MaqsSpecFlowExtension\ProjectTemplates\Magenic SpecFlow Test" $PSScriptRoot"\Extensions\MaqsSpecFlowExtension\ProjectTemplates\Magenic SpecFlow Test"
        ZipFiles $PSScriptRoot"\Extensions\MaqsSpecFlowExtension\ProjectTemplates\Magenic SpecFlow Test\NUnit" $PSScriptRoot"\Extensions\MaqsSpecFlowExtension\ProjectTemplates\Magenic SpecFlow Test\NUnit"
        ZipFiles $PSScriptRoot"\Extensions\MaqsSpecFlowExtension\ItemTemplates\Magenic SpecFlow Test" $PSScriptRoot"\Extensions\MaqsSpecFlowExtension\ItemTemplates\Magenic SpecFlow Test"
    }
}

WorkflowFunction $closedSource $openSource $specSource