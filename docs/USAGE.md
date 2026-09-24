# Usage — gregMod.KeyChaos

## Goal

Suffer beautifully. Every keypress re-deals your movement keys. Managing a
datacenter when W means "crouch" (until the next keypress, when it means
"sprint") builds character.

## Survival guide

1. Install the DLL (see [QUICKSTART.md](../QUICKSTART.md)). Chaos is ON by
   default.
2. Press any key. Notice your controls changed. That is the entire mod.
3. To stop the pain, press **Escape** (default panic key): vanilla bindings
   return instantly, chaos switches OFF. Escape still opens the pause menu
   as usual — two birds, one key.
4. Press Escape again to re-enable (it shuffles immediately, out of spite).
5. The bottom-left dot shows `KeyChaos: ON (shuffles: N)` or `OFF`. The
   mapping itself is never shown. Press keys to discover them. This is
   called gameplay.

## Rules of the chaos

- **Player actions only.** Move / Jump / Sprint / Crouch / Interact / Drop …
  get shuffled. Pause menus, dialogs, and mouse look are untouched.
- **Mod hotkeys keep working.** F1, F4, F7–F11 etc. poll the keyboard
  directly and ignore binding overrides. Your panels survive the chaos.
- **Every keypress counts** — including F-keys, including keys pressed in
  menus. Each one re-deals the deck.
- **Holding a key** fires one shuffle (no auto-repeat in the Input System),
  so you can hold your (current) forward key to walk.
- **Panic key never shuffles.** Change it in `MelonPreferences.cfg`
  (`gregMod.KeyChaos` → `PanicKey`, e.g. `F5`); F1 cannot edit key names.

## Settings reference

| Setting | F1 (gregCore) | Prefs | Default | Meaning |
|---|---|---|---|---|
| Chaos enabled | yes | yes | ON | Master switch |
| Status dot | yes | yes | ON | ON/OFF + counter, never the mapping |
| Panic key | — | yes | `Escape` | Restores defaults + toggles, never shuffled |

## Disclaimers

- Do not enable before learning the base controls. Or do. It changes nothing.
- Speedrunning the tutorial with chaos on is a valid achievement. There is
  no leaderboard. There will never be one.
