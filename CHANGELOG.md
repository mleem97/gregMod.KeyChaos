# Changelog — gregMod.KeyChaos

Format: [Keep a Changelog](https://keepachangelog.com/en/1.0.0/). Version: see [`VERSION`](VERSION).

## [Unreleased]

## [0.1.0] — 2026-09-24

### Added

- Initial release: per-keypress permutation of Player-map keyboard bindings
  via Input System binding overrides.
- Panic key (default Escape, configurable via prefs): restores vanilla
  bindings and toggles chaos off/on, never shuffled itself.
- Blind-by-design status dot (ON/OFF + shuffle counter, mapping never shown).
- F1 config entries (`Enabled`, `StatusOverlay`) with MelonPreferences
  fallback; soft gregCore dependency (loads without it).
- Zero Harmony patches; restore-on-unload; fail-safe passthrough when no
  Player-map keyboard slots are found.
- Docs: `docs/USAGE.md`, `docs/ARCHITECTURE.md`, `docs/COMPATIBILITY.md`.
