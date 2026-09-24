# scripts — gregMod.KeyChaos

Build/helper scripts.

Back: [README.md](../README.md) · Docs: [docs/INDEX.md](../docs/INDEX.md).

Builds run from the repository root with the shared helper:

```bash
# from ModRepositories/
./build.sh KeyChaos            # Release build
./build.sh KeyChaos --deploy   # build + copy DLL to Data Center/Mods
```

`tools/sync-melon-assemblies.sh` keeps `references/*.dll` pointed at the live
game assemblies before building.
