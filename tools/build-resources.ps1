param(
    [Parameter(Mandatory)][string]$project
)

$root = $(Get-Item $PSScriptRoot).Parent.FullName
$projectRoot = $(Get-Item $(Join-Path $root $project))

$loc = Get-Location

Set-Location $(Join-Path $projectRoot "Resources")
& $(Join-Path ${env:ProgramFiles(x86)} "Windows Kits\10\bin\10.0.22000.0\x64\rc.exe") .\resources.rc
Set-Location $loc
