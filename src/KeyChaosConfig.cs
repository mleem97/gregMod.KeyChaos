using System;
using MelonLoader;
using UnityEngine.InputSystem;

namespace GregMod.KeyChaos
{
    /// <summary>
    /// Configuration for gregMod.KeyChaos.
    ///
    /// Two-layer model:
    ///  1. MelonPreferences (always available, editable in MelonPreferences.cfg).
    ///  2. gregCore ModConfigSystem / F1 config UI (when gregCore is installed),
    ///     which takes precedence at read time.
    ///
    /// The panic key is prefs-only (the F1 config UI exposes bool/int/float
    /// entries; key names stay a text pref). It is NEVER shuffled.
    /// </summary>
    internal static class KeyChaosConfig
    {
        internal const string ModId = "gregMod.KeyChaos";

        /// <summary>Action map whose keyboard bindings get shuffled.</summary>
        internal const string TargetMap = "Player";

        private static MelonPreferences_Category _cat;
        private static MelonPreferences_Entry<bool> _enabled;
        private static MelonPreferences_Entry<bool> _statusOverlay;
        private static MelonPreferences_Entry<string> _panicKey;

        internal static Key PanicKey = Key.Escape;

        internal static void Load()
        {
            try
            {
                _cat = MelonPreferences.CreateCategory(ModId, "KeyChaos");
                _enabled = _cat.CreateEntry("Enabled", true, "Chaos enabled",
                    "Master switch. Every keypress reshuffles the Player-map keyboard bindings while on.");
                _statusOverlay = _cat.CreateEntry("StatusOverlay", true, "Status dot",
                    "Tiny ON/OFF indicator. The live mapping is NEVER shown (blind chaos).");
                _panicKey = _cat.CreateEntry("PanicKey", "Escape", "Panic key",
                    "Input System key that restores default bindings and toggles chaos off/on. Never shuffled.");
                _cat.SaveToFile(false);

                PanicKey = ParseKey(_panicKey.Value, Key.Escape);
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"[KeyChaos] Config load failed, using built-in defaults: {ex.GetBaseException().Message}");
            }
        }

        internal static bool Enabled => GetBool("Enabled", _enabled, true);

        internal static bool StatusOverlay => GetBool("StatusOverlay", _statusOverlay, true);

        internal static void SetEnabled(bool v)
        {
            if (_enabled == null) return;
            _enabled.Value = v;
            MelonPreferences.Save();
        }

        internal static Key ParseKey(string raw, Key fallback)
        {
            try
            {
                if (Enum.TryParse<Key>(raw, true, out var k) && k != Key.None)
                    return k;
            }
            catch { /* fall through */ }
            MelonLogger.Warning($"[KeyChaos] Unknown key '{raw}', defaulting to {fallback}.");
            return fallback;
        }

        private static bool GetBool(string key, MelonPreferences_Entry<bool> pref, bool fallback)
        {
            // F1 (gregCore ModConfigSystem) takes precedence when available.
            if (GregHost.HasCore)
            {
                try { return KeyChaosCoreConfig.GetBoolValue(ModId, key, pref != null ? pref.Value : fallback); }
                catch { /* fall through to prefs */ }
            }
            try { return pref != null ? pref.Value : fallback; }
            catch { return fallback; }
        }

        // ── gregCore registration (called only when HasCore; own type for JIT split)
        internal static void RegisterF1Entries()
        {
            try { KeyChaosCoreConfig.RegisterEntries(); }
            catch (Exception ex)
            {
                MelonLogger.Warning("[KeyChaos] F1 config registration failed: " + ex.GetBaseException().Message);
            }
        }
    }
}
