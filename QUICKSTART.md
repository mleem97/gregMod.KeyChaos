# Quickstart — gregMod.KeyChaos

> Every keypress reshuffles your controls. You were warned.

Repo: [https://github.com/mleem97/gregMod.KeyChaos](https://github.com/mleem97/gregMod.KeyChaos) · Version: `0.1.0` · License: Apache-2.0.

## 1. Clone

```bash
git clone https://github.com/mleem97/gregMod.KeyChaos.git
cd gregMod.KeyChaos
```

## 2. Build

```bash
# Sync game/loader assemblies first (repo root helper)
../ModRepositories/tools/sync-melon-assemblies.sh

dotnet build gregMod.KeyChaos.csproj -c Release
```

The DLL lands in `bin/Release/net6.0/gregMod.KeyChaos.dll`.

## 3. Install

Copy the DLL to the game Mods folder:

```bash
# Linux example
cp bin/Release/net6.0/gregMod.KeyChaos.dll \
  "$HOME/.local/share/Steam/steamapps/common/Data Center/Mods/"
```

(Or from the repo root: `./build.sh KeyChaos --deploy`.)

## 4. Use

1. Start the game. Chaos is ON by default (F1 → Mod Config →
   `gregMod.KeyChaos` → `Chaos enabled`, or `MelonPreferences.cfg`).
2. Press any key. Your controls just changed. Press another. Changed again.
3. Panic? Press **Escape**: vanilla bindings return, chaos switches OFF.
   Press Escape again to dive back in.
4. The bottom-left dot shows ON/OFF + shuffle count. The mapping itself is
   never shown. That's the game.

Details: [README.md](README.md), [docs/USAGE.md](docs/USAGE.md).
If you run into problems: file an issue
([Issues](https://github.com/mleem97/gregMod.KeyChaos/issues)) or read
[CONTRIBUTING.md](CONTRIBUTING.md).
