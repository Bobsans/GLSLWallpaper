#define MyAppName "GLSLWallpaper"
#define MyAppVersion "v1.1.1"
#define MyAppPublisher "Darkboy, Inc."
#define MyAppURL "https://github.com/Bobsans/GLSLWallpaper"
#define MyAppExeName "GLSLWallpaper.exe"
#define MyAppPreviewExeName "GLSLWallpaper.Preview.exe"

[Setup]
AppId={{631961D4-5118-43A8-8DE0-7638945821A3}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
AllowNoIcons=yes
OutputDir=.
OutputBaseFilename={#MyAppName} {#MyAppVersion} setup
SetupIconFile=..\GLSLWallpaper\Resources\icon.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern
ChangesAssociations=yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "russian"; MessagesFile: "compiler:Languages\Russian.isl"

[CustomMessages]
english.AdditionalTasks=Additional tasks:
russian.AdditionalTasks=Дополнительно:
english.AssociateFiles=Associate files ".glslwallpaper" with application
russian.AssociateFiles=Ассоциировать файлы ".glslwallpaper" с программой
english.AssociateFragFiles=Associate files ".frag" with application
russian.AssociateFragFiles=Ассоциировать файлы ".frag" с программой
english.WallpaperFileType=GLSL wallpaper file
russian.WallpaperFileType=Файл обоев GLSL
english.ShaderFileType=Vertex shader file
russian.ShaderFileType=Файл вертексного шейдера

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"
Name: "associate"; Description: "{cm:AssociateFiles}"; GroupDescription: "{cm:AdditionalTasks}"
Name: "associatefrag"; Description: "{cm:AssociateFragFiles}"; GroupDescription: "{cm:AdditionalTasks}"

[Files]
Source: "app\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Dirs]
Name: "{app}"; Permissions: everyone-full
Name: "{app}\Packs"; Permissions: everyone-full

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[Registry]
Root: HKCR; Subkey: ".glslwallpaper"; ValueType: string; ValueName: ""; ValueData: "{#MyAppName}"; Flags: uninsdeletevalue; Tasks: associate
Root: HKCR; Subkey: "{#MyAppName}"; ValueType: string; ValueName: ""; ValueData: "{cm:WallpaperFileType}"; Flags: uninsdeletekey; Tasks: associate
Root: HKCR; Subkey: "{#MyAppName}\DefaultIcon"; ValueType: string; ValueName: ""; ValueData: "{app}\{#MyAppExeName},1"; Tasks: associate
Root: HKCR; Subkey: "{#MyAppName}\shell\open\command"; ValueType: string; ValueName: ""; ValueData: """{app}\{#MyAppExeName}"" ""%1"""; Tasks: associate
Root: HKCR; Subkey: ".frag"; ValueType: string; ValueName: ""; ValueData: "{#MyAppName}.Preview"; Flags: uninsdeletevalue; Tasks: associatefrag
Root: HKCR; Subkey: "{#MyAppName}.Preview"; ValueType: string; ValueName: ""; ValueData: "{cm:ShaderFileType}"; Flags: uninsdeletekey; Tasks: associatefrag
Root: HKCR; Subkey: "{#MyAppName}.Preview\DefaultIcon"; ValueType: string; ValueName: ""; ValueData: "{app}\{#MyAppPreviewExeName},0"; Tasks: associatefrag
Root: HKCR; Subkey: "{#MyAppName}.Preview\shell\open\command"; ValueType: string; ValueName: ""; ValueData: """{app}\{#MyAppPreviewExeName}"" ""%1"" --watch"; Tasks: associatefrag

[UninstallDelete]
Type: files; Name: "{app}\latest.log"
