import os
import shutil
from zipfile import ZipFile

CWD = os.path.dirname(__file__)

OUT_RELEASE_PATH = os.path.join(CWD, 'release')
OUT_RELEASE_DATA_PATH = os.path.join(OUT_RELEASE_PATH, 'data')

BINARIES_PATH = os.path.join(CWD, 'bin', 'Release')

if os.path.exists(OUT_RELEASE_PATH):
    shutil.rmtree(OUT_RELEASE_PATH)
os.makedirs(OUT_RELEASE_PATH)

for file in ('GLSLWallpapers.exe', 'INIFileParser.dll', 'OpenTK.dll'):
    shutil.copyfile(os.path.join(BINARIES_PATH, file), os.path.join(OUT_RELEASE_PATH, file))

archive_files = ('info.ini', 'shader.glsl', 'thumbnail.png')
release_shaders_path = os.path.join(OUT_RELEASE_DATA_PATH, 'shaders')

if not os.path.exists(release_shaders_path):
    os.makedirs(release_shaders_path)

shader_source_path = os.path.join(CWD, 'data', 'shaders')

for path in os.listdir(shader_source_path):
    if os.path.isdir(os.path.join(shader_source_path, path)):
        if not all(os.path.exists(os.path.join(shader_source_path, path, f)) for f in archive_files):
            print(f'Invalid sahder folder {path}')
        else:
            with ZipFile(os.path.join(release_shaders_path, f'{path}.glslwallpaper'), 'w') as zip:
                for file in archive_files:
                    zip.write(os.path.join(shader_source_path, path, file), file)
                    
shutil.copyfile(os.path.join(CWD, 'data', 'placeholder.png'), os.path.join(OUT_RELEASE_DATA_PATH, 'placeholder.png'))
