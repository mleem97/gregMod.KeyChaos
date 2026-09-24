# gregMod.KeyChaos

> Every keypress reshuffles your controls. You were warned.

Copy `gregMod.KeyChaos.dll` to `Data Center/Mods/`.

![License](https://img.shields.io/github/license/mleem97/gregMod.KeyChaos?style=for-the-badge)

## Links

- **Repository:** [https://github.com/mleem97/gregMod.KeyChaos](https://github.com/mleem97/gregMod.KeyChaos)
- **Issues:** [https://github.com/mleem97/gregMod.KeyChaos/issues](https://github.com/mleem97/gregMod.KeyChaos/issues)
- **Releases:** [https://github.com/mleem97/gregMod.KeyChaos/releases](https://github.com/mleem97/gregMod.KeyChaos/releases)

## Overview

**gregMod.KeyChaos** permutes the game's Player-map keyboard bindings on
**every keypress** via Unity Input System binding overrides. Walk with W?
That was before. Press it again — now it's Crouch. Good luck managing cables
like that.

Safety rails (this is chaos, not a brick):

- The **panic key** (default **Escape**) is never shuffled. Pressing it
  restores vanilla bindings and toggles chaos off (press again to re-enable).
- **Blind chaos:** the live mapping is never displayed — only an ON/OFF
  status dot with a shuffle counter.
- Only Player-map actions shuffle. Pause menus, UI dialogs, and all mod
  hotkeys (direct keyboard polling) keep working.
- Disabling or unloading the mod restores vanilla bindings.

See [docs/INDEX.md](docs/INDEX.md) for the complete documentation, and
[docs/USAGE.md](docs/USAGE.md) for survival tips.

## Compatibility

| Platform    | Status    |
| ----------- | --------- |
| Windows x64 | Supported |
| Linux x64   | Supported |

Game/loader baseline: see [docs/COMPATIBILITY.md](docs/COMPATIBILITY.md).

## Features

- Per-keypress Fisher-Yates permutation of Player keyboard bindings.
- Panic key with instant restore (default Escape, configurable).
- Blind-by-design status dot (shuffle counter included, mapping never shown).
- F1 config entries (gregCore) + MelonPreferences fallback, no gregCore needed.
- Zero Harmony patches — pure Input System rebinding + polling.

## Installation

See [QUICKSTART.md](QUICKSTART.md).

## Build from Source

```bash
git clone https://github.com/mleem97/gregMod.KeyChaos.git
cd gregMod.KeyChaos
dotnet build gregMod.KeyChaos.csproj -c Release
```

Details: [QUICKSTART.md](QUICKSTART.md), [CONTRIBUTING.md](CONTRIBUTING.md).

## Repository Layout

```
├── README.md            # This file
├── QUICKSTART.md        # Quickstart
├── CHANGELOG.md         # Changelog (Keep a Changelog)
├── CONTRIBUTING.md      # Contributing
├── SECURITY.md          # Security reports
├── CODE_OF_CONDUCT.md   # Code of conduct
├── AGENTS.md            # Notes for AI agents
├── LICENSE              # Apache-2.0
├── VERSION              # Single source of truth for the version
├── manifest.json        # Mod manifest
├── docs/                # Documentation ([Index](docs/INDEX.md))
├── scripts/             # Build/helper scripts
├── tests/               # Tests
├── references/          # Game/loader assemblies (symlinks, never committed)
├── examples/            # Examples
└── src/                 # C# source
```

## API Documentation

See [`docs/INDEX.md`](docs/INDEX.md).

## Credits

| Role       | Contributor                                        |
| ---------- | -------------------------------------------------- |
| **Codebase** | [mleem97](https://github.com/mleem97)            |

## Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md).

## License

Apache-2.0 — see [`LICENSE`](LICENSE).

## 🚀 Join the gregFramework Team!

Do you enjoy building mods, tools, or docs? Get in touch: **apply@gregframework.eu** or via
[Discord](https://discord.gg/greg) — Code, Assets, Docs, Testing, Infra, Community.

---

**gregFramework — powered by the community.**
