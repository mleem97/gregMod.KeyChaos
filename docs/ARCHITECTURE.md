# Architecture — gregMod.KeyChaos

> Poll keys, permute bindings, panic restores. No patches, no mercy.

## Components

```text
any key ──► KeyChaosMod.OnUpdate ──► KeyShuffler.ShuffleOnce ──► Input System overrides
   │                │                          │                            │
   │ poll           │ panic? restore+toggle    │ Player map, <Keyboard>/    │ game reads new
   │ Keyboard       │ else shuffle             │ paths, Fisher-Yates        │ bindings live
```

| File | Responsibility |
|---|---|
| `src/KeyChaosMod.cs` | MelonMod entry: config load, per-frame key poll, panic toggle, status dot (`OnGUI`), restore-on-unload |
| `src/KeyChaosConfig.cs` | Two-layer config: MelonPreferences always; gregCore F1 wins when present; `PanicKey` parsing |
| `src/KeyChaosCoreConfig.cs` | **Only** file with direct gregCore references (JIT split behind `GregHost.HasCore`): F1 entries + mod registry |
| `src/GregHost.cs` | Soft-dependency probe (`Type.GetType`, no hard load) |
| `src/KeyShuffler.cs` | Slot collection (enabled Player-map keyboard bindings minus panic), Fisher-Yates permutation, `ApplyBindingOverride` write-back, `RemoveAllBindingOverrides` restore |
| `src/MyPluginInfo.cs` | Plugin id / name / version constants |

## Data flows

1. **Detect:** `OnUpdate` reads `Keyboard.current`; `anyKey.wasPressedThisFrame`
   gates, panic key checked first (toggle path, returns early).
2. **Shuffle:** `ListEnabledActions()` → filter map `Player` → leaf bindings
   with `<Keyboard>/` effective path, minus panic path → permute paths →
   `ApplyBindingOverride(action, bindingIndex, path)` per slot. Touched
   actions recorded for restore.
3. **Panic:** `RemoveAllBindingOverrides` on touched actions → prefs
   `Enabled=false` (or re-enable + immediate shuffle).
4. **Display:** `OnGUI` draws one `GUI.Label` (status + counter). Mapping
   never leaves the engine.

## Failure handling

- No Player-map keyboard slots → warning + no-op (vanilla passthrough).
- Per-action/per-slot try/catch everywhere; stale actions (scene change)
  dropped silently on restore.
- Missing seams: none — there are no Harmony patches to miss. Worst case is
  an API drift compile error, caught by the build.

Record changes here + [`CHANGELOG.md`](../CHANGELOG.md) (Unreleased).
