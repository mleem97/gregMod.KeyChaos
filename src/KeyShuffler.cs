using System;
using System.Collections.Generic;
using MelonLoader;
using UnityEngine.InputSystem;

namespace GregMod.KeyChaos
{
    /// <summary>
    /// Shuffle engine: permutes keyboard binding paths across the game's
    /// enabled Player-map actions.
    ///
    /// How it works:
    ///  - Collect every enabled action whose map is <see cref="KeyChaosConfig.TargetMap"/>
    ///    ("Player") and gather its leaf bindings whose effective path is a
    ///    single keyboard key ("&lt;Keyboard&gt;/w", composite parts included).
    ///  - The panic key's binding is NEVER touched (Escape stays Escape).
    ///  - Fisher-Yates the collected paths and write them back with
    ///    InputActionRebindingExtensions.ApplyBindingOverride(action, index, path).
    ///  - Restore = RemoveAllBindingOverrides on every touched action.
    ///
    /// Only Unity Input System actions are affected. Mod hotkeys that poll
    /// Keyboard.current directly (all Greg mods) keep working — the chaos is
    /// confined to actual gameplay input. No Harmony patches needed.
    /// </summary>
    internal static class KeyShuffler
    {
        private static readonly System.Random _rng = new System.Random();

        /// <summary>Actions currently carrying our overrides (for restore).</summary>
        private static readonly List<InputAction> _touched = new List<InputAction>();

        internal static int ShuffleCount { get; private set; }
        internal static int LastSlotCount { get; private set; }

        private struct Slot
        {
            internal InputAction Action;
            internal int BindingIndex;
            internal string Path;
        }

        /// <summary>Permutes keyboard paths across Player-map actions. Returns slots shuffled.</summary>
        internal static int ShuffleOnce()
        {
            List<Slot> slots;
            try
            {
                slots = CollectSlots();
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"[KeyChaos] Slot scan failed: {ex.Message}");
                return 0;
            }

            LastSlotCount = slots.Count;
            if (slots.Count < 2)
            {
                MelonLogger.Warning($"[KeyChaos] Only {slots.Count} keyboard slot(s) in map '{KeyChaosConfig.TargetMap}' — nothing to shuffle.");
                return 0;
            }

            // Fisher-Yates over the path multiset (duplicates survive as-is).
            var paths = new List<string>(slots.Count);
            for (int i = 0; i < slots.Count; i++) paths.Add(slots[i].Path);
            for (int i = paths.Count - 1; i > 0; i--)
            {
                int j = _rng.Next(i + 1);
                string tmp = paths[i];
                paths[i] = paths[j];
                paths[j] = tmp;
            }

            int applied = 0;
            for (int i = 0; i < slots.Count; i++)
            {
                try
                {
                    InputActionRebindingExtensions.ApplyBindingOverride(slots[i].Action, slots[i].BindingIndex, paths[i]);
                    Remember(slots[i].Action);
                    applied++;
                }
                catch (Exception ex)
                {
                    MelonLogger.Warning($"[KeyChaos] Override failed on '{SafeActionName(slots[i].Action)}': {ex.Message}");
                }
            }

            ShuffleCount++;
            MelonLogger.Msg($"[KeyChaos] Shuffled {applied}/{slots.Count} bindings (shuffle #{ShuffleCount}). Good luck.");
            return applied;
        }

        /// <summary>Removes all our overrides, restoring vanilla bindings.</summary>
        internal static void RestoreDefaults()
        {
            int restored = 0;
            for (int i = _touched.Count - 1; i >= 0; i--)
            {
                try
                {
                    var a = _touched[i];
                    if (a != null)
                    {
                        InputActionRebindingExtensions.RemoveAllBindingOverrides(a);
                        restored++;
                    }
                }
                catch { /* action may be gone after scene change — drop it */ }
            }
            _touched.Clear();
            if (restored > 0)
                MelonLogger.Msg($"[KeyChaos] Restored default bindings on {restored} action(s).");
        }

        // ── Internals ────────────────────────────────────────────────────────

        private static List<Slot> CollectSlots()
        {
            var slots = new List<Slot>();
            Il2CppSystem.Collections.Generic.List<InputAction> enabled;
            try
            {
                enabled = InputSystem.ListEnabledActions();
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"[KeyChaos] ListEnabledActions failed: {ex.Message}");
                return slots;
            }
            if (enabled == null) return slots;

            string panicPath = "<Keyboard>/" + KeyChaosConfig.PanicKey.ToString().ToLowerInvariant();

            for (int a = 0; a < enabled.Count; a++)
            {
                InputAction action;
                try { action = enabled[a]; }
                catch { continue; }
                if (action == null) continue;

                try
                {
                    if (!action.enabled) continue;
                    var map = action.actionMap;
                    if (map == null) continue;
                    string mapName = map.name ?? "";
                    if (!string.Equals(mapName, KeyChaosConfig.TargetMap, StringComparison.OrdinalIgnoreCase))
                        continue;

                    var bindings = action.bindings;
                    int count;
                    try { count = bindings.Count; }
                    catch { continue; }
                    for (int b = 0; b < count; b++)
                    {
                        InputBinding binding;
                        try { binding = bindings[b]; }
                        catch { continue; }
                        string path;
                        try
                        {
                            if (binding.isComposite) continue;
                            path = binding.effectivePath ?? "";
                        }
                        catch { continue; }
                        if (!path.StartsWith("<Keyboard>/", StringComparison.OrdinalIgnoreCase))
                            continue;
                        if (string.Equals(path, panicPath, StringComparison.OrdinalIgnoreCase))
                            continue; // the panic key is sacred
                        slots.Add(new Slot { Action = action, BindingIndex = b, Path = path });
                    }
                }
                catch { /* per-action best-effort */ }
            }
            return slots;
        }

        private static void Remember(InputAction action)
        {
            try
            {
                for (int i = 0; i < _touched.Count; i++)
                {
                    try { if (_touched[i] == action) return; }
                    catch { /* stale entry, keep going */ }
                }
                _touched.Add(action);
            }
            catch { /* best-effort */ }
        }

        private static string SafeActionName(InputAction action)
        {
            try { return action != null ? (action.name ?? "?") : "?"; }
            catch { return "?"; }
        }
    }
}
