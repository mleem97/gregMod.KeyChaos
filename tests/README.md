# tests — gregMod.KeyChaos

Tests, fixtures, and test documentation.

Back: [README.md](../README.md) · Docs: [docs/INDEX.md](../docs/INDEX.md).

## What can be tested without the game

- `dotnet build gregMod.KeyChaos.csproj -c Release` must stay at
  **0 warnings, 0 errors** (Input System API drift shows up here first).

## In-game checklist (see docs/COMPATIBILITY.md)

1. Mod loads; status dot visible; no console errors.
2. Keypress reshuffles movement (3 presses, 3 mappings).
3. Escape restores vanilla bindings + disables; Escape again re-enables.
4. Unload/disable leaves no stuck overrides.
5. With gregCore (F1) and without (prefs).
