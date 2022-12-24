import os
import subprocess

task = lambda s: f'-> \u001b[38;5;10m\u001b[1m{s}\u001b[0m'
info = lambda s: f'  | \u001b[38;5;8m{s}\u001b[0m'
ok = lambda: f'\u001b[38;5;10m\u001b[1mOK!\u001b[0m'

CWD = os.path.dirname(__file__)
ROOT = os.path.dirname(CWD)
OUT_RELEASE_PATH = os.path.join(CWD, 'out')
OUT_RELEASE_PACKS_PATH = os.path.join(OUT_RELEASE_PATH, 'Packs')
PACKS_SOURCES_PATH = os.path.join(ROOT, 'Packs')
PACKER_PATH = os.path.join(ROOT, 'GLSLWallpaper.Packer', 'bin', 'Debug', 'net7.0-windows', 'GLSLWallpaper.Packer.exe')

if not os.path.exists(OUT_RELEASE_PATH):
    os.makedirs(OUT_RELEASE_PATH, exist_ok=True)

print(task('Building project...'))
p = subprocess.Popen(['dotnet', 'publish', os.path.join(ROOT, 'GLSLWallpaper', 'GLSLWallpaper.csproj'), '-a', 'x64', '-c', 'Release', '--no-self-contained', '-o', OUT_RELEASE_PATH], stdout=subprocess.PIPE)
while p.poll() is None:
    if line := p.stdout.readline().decode().strip():
        print(info(line))

print(task('Packing packs...'))
p = subprocess.Popen([PACKER_PATH, 'pack', '--input', PACKS_SOURCES_PATH, '--output', OUT_RELEASE_PACKS_PATH], stdout=subprocess.PIPE)
while p.poll() is None:
    if line := p.stdout.readline().decode().strip():
        print(info(line))

print(ok())
