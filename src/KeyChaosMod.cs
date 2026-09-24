using System;
using MelonLoader;
using UnityEngine;
using UnityEngine.InputSystem;

[assembly: MelonInfo(typeof(GregMod.KeyChaos.KeyChaosMod),
    GregMod.KeyChaos.MyPluginInfo.PLUGIN_NAME,
    GregMod.KeyChaos.MyPluginInfo.PLUGIN_VERSION,
    GregMod.KeyChaos.MyPluginInfo.PLUGIN_AUTHOR)]
[assembly: MelonGame("Waseku", "Data Center")]

namespace GregMod.KeyChaos
{
    /// <summary>
    /// gregMod.KeyChaos — every keypress reshuffles the game's Player-map
    /// keyboard bindings. Walk with W? That was before. Good luck.
    ///
    /// Rules:
    ///  - The panic key (default Escape) is NEVER shuffled. Pressing it
    ///    restores vanilla bindings and toggles chaos off (press again to
    ///    re-enable). It still reaches the game (e.g. pause menu) as usual.
    ///  - Blind chaos: the live mapping is never displayed, only an ON/OFF
    ///    status dot. Discover your keys by pressing them.
    ///  - Only Unity Input System actions in the Player map are affected.
    ///    Mod hotkeys (direct keyboard polling) keep working.
    ///
    /// Toggle details in F1 (gregCore) or MelonPreferences.cfg.
    /// </summary>
    public sealed class KeyChaosMod : MelonMod
    {
        public override void OnInitializeMelon()
        {
            try
            {
                KeyChaosConfig.Load();

                if (GregHost.HasCore)
                {
                    try
                    {
                        KeyChaosConfig.RegisterF1Entries();
                        KeyChaosCoreConfig.RegisterMod(MyPluginInfo.PLUGIN_VERSION);
                    }
                    catch (Exception ex)
                    {
                        LoggerInstance.Warning($"[KeyChaos] gregCore wiring failed: {ex.GetBaseException().Message}");
                    }
                }

                LoggerInstance.Msg(
                    $"[KeyChaos] {MyPluginInfo.PLUGIN_VERSION} loaded. " +
                    $"Enabled={KeyChaosConfig.Enabled}, Panic={KeyChaosConfig.PanicKey}. " +
                    "Every keypress reshuffles your controls. You were warned.");
            }
            catch (Exception ex)
            {
                LoggerInstance.Error($"[KeyChaos] Startup failed: {ex.GetBaseException().Message}");
            }
        }

        public override void OnUpdate()
        {
            Keyboard kb;
            try
            {
                kb = Keyboard.current;
                if (kb == null) return;
            }
            catch { return; }

            bool panicPressed;
            bool anyPressed;
            try
            {
                var panic = kb[KeyChaosConfig.PanicKey];
                panicPressed = panic != null && panic.wasPressedThisFrame;
                anyPressed = kb.anyKey != null && kb.anyKey.wasPressedThisFrame;
            }
            catch { return; }

            // Panic key first: restore defaults and toggle. It is never shuffled.
            if (panicPressed)
            {
                try
                {
                    if (KeyChaosConfig.Enabled)
                    {
                        KeyShuffler.RestoreDefaults();
                        KeyChaosConfig.SetEnabled(false);
                        LoggerInstance.Msg("[KeyChaos] Panic! Vanilla bindings restored, chaos OFF. Press panic again to re-enable.");
                    }
                    else
                    {
                        KeyChaosConfig.SetEnabled(true);
                        KeyShuffler.ShuffleOnce();
                        LoggerInstance.Msg("[KeyChaos] Chaos re-enabled. Your keys are already wrong.");
                    }
                }
                catch (Exception ex)
                {
                    LoggerInstance.Warning($"[KeyChaos] Panic toggle failed: {ex.Message}");
                }
                return; // the panic press itself never shuffles
            }

            if (!KeyChaosConfig.Enabled) return;
            if (!anyPressed) return;

            try { KeyShuffler.ShuffleOnce(); }
            catch (Exception ex)
            {
                LoggerInstance.Warning($"[KeyChaos] Shuffle failed: {ex.Message}");
            }
        }

        public override void OnGUI()
        {
            try
            {
                if (!KeyChaosConfig.StatusOverlay) return;
                string text = KeyChaosConfig.Enabled
                    ? $"KeyChaos: ON (shuffles: {KeyShuffler.ShuffleCount}) — {KeyChaosConfig.PanicKey} stops the pain"
                    : "KeyChaos: OFF";
                GUI.Label(new Rect(10, Screen.height - 28, 600, 20), text);
            }
            catch { /* overlay best-effort */ }
        }

        public override void OnDeinitializeMelon()
        {
            try { KeyShuffler.RestoreDefaults(); }
            catch { /* best-effort */ }
        }
    }
}
