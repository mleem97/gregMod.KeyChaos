# Compatibility — gregMod.KeyChaos

## Baseline (built and signature-checked against)

- Game: **Data Center** by **Waseku** (`app.info`: `Waseku / Data Center`).
- Interop assemblies dated **2026-09-19** (`MelonLoader/Il2CppAssemblies`,
  `GameAssembly.dll` of the local Steam install).
- Loader: **MelonLoader 0.7.x**, mod target **net6.0-x64**.
- `gregCore` (optional): soft dependency only — mod loads and works without it.
- Platforms: Windows x64 / Linux x64.

## Reverse-engineering evidence

Game input — generated Input System wrapper (in `Assembly-CSharp`):

- Action getters observed: `Move, Jump, Sprint, Crouch, Interact, Drop, Look,
  Scroll, Zoom` (+ UI: `Submit, Cancel, Pause`), interfaces `IPlayerActions` /
  `IUIActions`, implicit `PlayerActions` / `UIActions` conversions.
- Target map name assumed `"Player"` (from the generated `PlayerActions`
  struct). If a game update renames it, the mod degrades to passthrough with
  a console warning — verify on drift.

Rebinding API — `UnityEngine.InputSystem` (`Unity.InputSystem.dll`):

- `InputActionRebindingExtensions.ApplyBindingOverride(InputAction, Int32,
  String)` — static extension, used for write-back. (Rebinding lives in the
  extension class, NOT on `InputAction` itself — the dummy `InputAction` has
  104 methods and none of them is an override API.)
- `InputActionRebindingExtensions.RemoveAllBindingOverrides(InputAction)` —
  restore path.
- `InputSystem.ListEnabledActions()` → `Il2Cpp List<InputAction>` — slot scan.
- `InputAction.bindings` → `ReadOnlyArray<InputBinding>` (action-relative
  indices, which is what the `(InputAction, int, …)` overloads expect).
- `InputBinding`: `path`, `effectivePath`, `overridePath`, `isComposite`,
  `isPartOfComposite` — all present.

> The interop dummies contain **no IL**. Override write-back order effects
> (e.g. composite re-resolution timing) are runtime behaviour — the mod
> re-reads `effectivePath` every shuffle, so drift self-corrects on the next
> keypress.

## Known limits (v1)

1. **In-game verification pending** — build passes (`0 warnings, 0 errors`);
   the live Player-map name, slot count, and shuffle feel must be confirmed
   in a running game (see `tests/README.md`).
2. **UI map untouched by design** — Pause/Submit/Cancel never shuffle.
3. **Mouse untouched by design** — look/aim bindings keep working.
4. **Duplicate keys possible** — permutation preserves the path multiset, so
   two actions may share a key after a shuffle (extra funny, documented).
5. **Panic key reaches the game too** — Escape opens the pause menu AND
   toggles chaos. Both happen. This is fine.

## Test status

- [x] `dotnet build -c Release` — clean (0 warnings, 0 errors).
- [ ] Mod loads; status dot visible; no console errors.
- [ ] Keypress reshuffles movement (spot-check 3 presses, 3 mappings).
- [ ] Escape restores vanilla bindings (verify W = forward again) + disables.
- [ ] Re-enable via Escape shuffles immediately.
- [ ] Unload/disable restores bindings (no stuck overrides).
- [ ] With and without gregCore (F1 entries vs prefs).
