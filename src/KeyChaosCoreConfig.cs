using System;
using MelonLoader;

namespace GregMod.KeyChaos
{
    /// <summary>
    /// All direct gregCore references live in this class and this class only.
    /// It is called exclusively behind <see cref="GregHost.HasCore"/>, so the
    /// mod still loads when gregCore is absent (JIT split: referencing methods
    /// are never jitted without gregCore present).
    ///
    /// Covers F1 config UI entries (DataCenterModLoader.ModConfigSystem) and
    /// the mod registry entry. No HUD/panel opener: this mod has no panel —
    /// the panic key is the entire UI.
    /// </summary>
    internal static class KeyChaosCoreConfig
    {
        internal static void RegisterEntries()
        {
            DataCenterModLoader.ModConfigSystem.RegisterBool(
                KeyChaosConfig.ModId, "Enabled", "Chaos enabled", true,
                "Master switch. Every keypress reshuffles the Player-map keyboard bindings while on.");
            DataCenterModLoader.ModConfigSystem.RegisterBool(
                KeyChaosConfig.ModId, "StatusOverlay", "Status dot", true,
                "Tiny ON/OFF indicator. The live mapping is never shown (blind chaos).");
        }

        internal static bool GetBoolValue(string modId, string key, bool fallback)
        {
            try { return DataCenterModLoader.ModConfigSystem.GetBoolValue(modId, key, fallback); }
            catch { return fallback; }
        }

        internal static void RegisterMod(string version)
        {
            try
            {
                gregCore.Core.Mods.GregModRegistry.Register(
                    KeyChaosConfig.ModId, "KeyChaos", version,
                    new string[] { "keychaos" });
            }
            catch (Exception ex)
            {
                MelonLogger.Warning("[KeyChaos] Mod registration failed: " + ex.GetBaseException().Message);
            }
        }
    }
}
