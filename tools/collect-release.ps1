param(
    [Switch]$keepVersion
)

$loc = Get-Location
$root = $(Get-Item $PSScriptRoot).Parent.FullName
$outPath = Join-Path $root "Build/app"

if (!$keepVersion.IsPresent) {
    & $(Join-Path $PSScriptRoot "bump-version.ps1") GLSLWallpaper
}

& $(Join-Path $PSScriptRoot "build-resources.ps1") GLSLWallpaper

if ((Get-Item $outPath -ErrorAction Ignore).Exists) {
    Remove-Item $outPath -Recurse -Force
}

dotnet publish $(Join-Path $root "GLSLWallpaper/GLSLWallpaper.csproj") -r win-x64 -c Release --no-self-contained -o $outPath
dotnet publish $(Join-Path $root "GLSLWallpaper.Preview/GLSLWallpaper.Preview.csproj") -r win-x64 -c Release --no-self-contained -o $outPath
dotnet publish $(Join-Path $root "GLSLWallpaper.Packer/GLSLWallpaper.Packer.csproj") -r win-x64 -c Release --no-self-contained -o $outPath

"output_path: .\Unpacked" | Set-Content $(Join-Path $outPath "config.yaml") -Encoding UTF8

& $(Join-Path $outPath "GLSLWallpaper.Packer.exe") pack --input $(Join-Path $root "Packs") --output $(Join-Path $outPath "Packs")

& $(Join-Path ${env:ProgramFiles(x86)} "Inno Setup 6\ISCC.exe") $(Join-Path $root "Build/setup.iss")

Set-Location $loc
