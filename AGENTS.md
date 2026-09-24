# AGENTS.md — Notes for AI agents (gregMod.KeyChaos)

Repo: [https://github.com/mleem97/gregMod.KeyChaos](https://github.com/mleem97/gregMod.KeyChaos) · License: Apache-2.0 · Version: see `VERSION`.

## Duties

1. **Read first:** `README.md`, `docs/INDEX.md`, `CONTRIBUTING.md` — only then make changes.
2. **Do not commit secrets** (keys, tokens, `.env`). Use keys only via environment variables.
3. **Preserve history:** no `push --force`, no history rewrite without instruction.
4. **Verify changes:** before reporting completion, build/test whatever the repo supports (`QUICKSTART.md`).
5. **Keep docs in sync:** for new features, update `README.md` + `docs/` + `CHANGELOG.md` (Unreleased).
6. **Conventions:** Conventional Commits (`feat:`, `fix:`, `docs:`, `chore:` …), one logical change per commit.
7. **When in doubt:** stop and ask instead of guessing — especially for deletes, migrations, CI.

## Mod-specific rules

- **The panic key is sacred.** It must never enter the shuffle pool
  (`KeyShuffler` excludes it by effective path) and must always restore
  defaults before toggling. Any change to trigger handling must preserve this.
- **Blind chaos is the design.** Never display the live mapping (no overlay,
  no log of paths). Status dot + shuffle counter only.
- **Scope is the Player action map.** Do not touch UI-map bindings (pause
  menus must stay usable) and never DisableAllEnabledActions or touch mouse
  paths.
- **Restore on every exit path:** panic-off, mod disable, unload
  (`OnDeinitializeMelon`). Use RemoveAllBindingOverrides only on actions the
  mod itself touched (`_touched` list).
- **gregCore is a soft dependency.** Direct references live only in
  `src/KeyChaosCoreConfig.cs` behind `GregHost.HasCore` (JIT split).
- **No Harmony in this mod.** Polling + Input System API only. If rebinding
  APIs ever prove insufficient, document the gap in COMPATIBILITY.md instead
  of reaching for patches first.
- **Rebinding APIs are extension methods** (`InputActionRebindingExtensions`),
  not members of `InputAction` — see docs/COMPATIBILITY.md evidence.

## Layout

See [README.md](README.md) → Repository Layout. Central entry points: `docs/INDEX.md`, `scripts/`, `tests/`.
Source: `src/` (`KeyChaosMod`, `KeyChaosConfig`, `KeyChaosCoreConfig`,
`KeyShuffler`, `GregHost`, `MyPluginInfo`).
