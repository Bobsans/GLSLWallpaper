param(
    [Parameter(Mandatory)][string]$project
)

$version = Get-Content $(Join-Path $project "version")
$versionParts = $version.Split(".")
#$versionParts[2] = [int]$versionParts[2] + 1
$newVersion = $versionParts -join "."

$root = $(Get-Item $PSScriptRoot).Parent.FullName

$projectRoot = $(Get-Item $(Join-Path $root $project))
$projectName = $projectRoot.Name

$file = Join-Path $projectRoot.FullName "$projectName.csproj"
$content = Get-Content $file
$content = $content -replace "(?<=<FileVersion>)($version)(?=</FileVersion>)", $newVersion
$content = $content -replace "(?<=<AssemblyVersion>)($version)(?=</AssemblyVersion>)", $newVersion
$content | Set-Content $file -Encoding UTF8

$file = Join-Path $projectRoot.FullName "Resources/app.manifest"
if ((Get-Item $file).Exists) {
    $content = Get-Content $file
    $content = $content -replace "(?<=version="")($version)(?="")", $newVersion
    $content | Set-Content $file -Encoding UTF8
}

$file = Join-Path $projectRoot.FullName "Resources/resources.rc"
if ((Get-Item $file).Exists) {
    $content = Get-Content $file
    $content = $content -replace "(?<=FILEVERSION\s+)($($version -replace "\.", ","))", $($newVersion -replace "\.", ",")
    $content = $content -replace "(?<=PRODUCTVERSION\s+)($($version -replace "\.", ","))", $($newVersion -replace "\.", ",")
    $content = $content -replace "(?<=""FileVersion"",\s+"")($version)(?="")", $newVersion
    $content = $content -replace "(?<=""ProductVersion"",\s+"")($version)(?="")", $newVersion
    $content = $content -replace "(?<=""AssemblyVersion"",\s+"")($version)(?="")", $newVersion
    $content | Set-Content $file -Encoding UTF8
}

$file = Join-Path $projectRoot.FullName "version"
$newVersion | Set-Content $file -Encoding UTF8

if ($projectName -eq "GLSLWallpaper") {
    $file = Join-Path $root "Build/setup.iss"
    $encoding = [System.Text.Encoding]::GetEncoding("windows-1251")
    $content = [System.IO.File]::ReadAllText($file, $encoding)
    $content = $content -replace "(?<=MyAppVersion\s+""v)($version)(?="")", $newVersion
    [System.IO.File]::WriteAllText($file, $content, $encoding)
}

