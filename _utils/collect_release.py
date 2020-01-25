import os
import shutil
from zipfile import ZipFile


def log(indent, message, end='\n'):
    print(f'%s%s{message}' % ('  ' * indent, '- ' if indent > 0 else ''), end=end)


CWD = os.path.dirname(os.path.dirname(__file__))

OUT_RELEASE_PATH = os.path.join(CWD, '_release', 'app')
OUT_RELEASE_DATA_PATH = os.path.join(OUT_RELEASE_PATH, 'data')

BINARIES_PATH = os.path.join(CWD, 'bin', 'Release')

if not os.path.exists(OUT_RELEASE_PATH):
    os.makedirs(OUT_RELEASE_PATH, exist_ok=True)

log(0, 'Copying files:')
for file in ('GLSLWallpapers.exe', 'INIFileParser.dll', 'OpenTK.dll', 'Sciter.dll', 'SciterSharpWindows.dll'):
    log(1, f'Copy {file}')
    shutil.copyfile(os.path.join(BINARIES_PATH, file), os.path.join(OUT_RELEASE_PATH, file))

archive_files = ('info.ini', 'thumbnail.png')
choice_files = ('shader.frag', 'shader.vert')

release_shaders_path = os.path.join(OUT_RELEASE_DATA_PATH, 'shaders')

if not os.path.exists(release_shaders_path):
    os.makedirs(release_shaders_path)

shader_source_path = os.path.join(CWD, 'data', 'shaders')

for path in os.listdir(shader_source_path):
    log(1, f'Packing {path}:')
    if os.path.isdir(os.path.join(shader_source_path, path)):
        if not all(os.path.exists(os.path.join(shader_source_path, path, f)) for f in archive_files) or not any(
            os.path.exists(os.path.join(shader_source_path, path, f)) for f in choice_files):
            log(2, f'Invalid sahder folder: {path}')
        else:
            with ZipFile(os.path.join(release_shaders_path, f'{path}.glslwallpaper'), 'w') as zip:
                for file in [*archive_files, *choice_files]:
                    file_path = os.path.join(shader_source_path, path, file)
                    if os.path.exists(file_path):
                        zip.write(file_path, file)
                        log(2, f'Collected {file}')

# COPY TO BIN FOR DEBUG

# APPS_PATH = 'E:/Applications/GLSLWallpapers v0.1'

log(0, 'Publishing into Applications folder... ', end='')
for root, folders, files in os.walk(OUT_RELEASE_DATA_PATH):
    for file in files:
        file_path = os.path.join(root, file)

        out_path = os.path.join(CWD, 'bin', 'Release', os.path.relpath(file_path, OUT_RELEASE_PATH))
        if not os.path.exists(out_path):
            os.makedirs(os.path.dirname(out_path), exist_ok=True)
        shutil.copy(os.path.join(root, file), out_path)

        out_path = os.path.join(CWD, 'bin', 'Debug', os.path.relpath(file_path, OUT_RELEASE_PATH))
        if not os.path.exists(out_path):
            os.makedirs(os.path.dirname(out_path), exist_ok=True)
        shutil.copy(os.path.join(root, file), out_path)

        # out_path = os.path.join(APPS_PATH, os.path.relpath(file_path, OUT_RELEASE_PATH))
        # if not os.path.exists(out_path):
        #     os.makedirs(os.path.dirname(out_path), exist_ok=True)
        # shutil.copy(os.path.join(root, file), out_path)
log(-1, 'OK!')
