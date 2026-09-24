# Source layout

All C# source lives in `src/`, current game/MelonLoader assemblies in `references/`
(symlinks into the local Data Center install — never commit DLLs), and project
documentation in `docs/`.

| File | Why it exists |
|---|---|
| `src/KeyChaosMod.cs` | MelonMod entry (poll, panic toggle, status dot, unload restore) |
| `src/MyPluginInfo.cs` | Plugin id/name/version constants |
| `src/GregHost.cs` | gregCore soft-dependency probe |
| `src/KeyChaosConfig.cs` | Effective config (F1 wins, prefs fallback, panic-key parsing) |
| `src/KeyChaosCoreConfig.cs` | Only file referencing gregCore (JIT split) |
| `src/KeyShuffler.cs` | Slot scan, Fisher-Yates permutation, override write-back, restore |
