# MoveOrDie

A "move or die" variant: a player who stands still for too long dies. Both the
delay and the minimum speed that counts as moving are adjustable.

A mod for **FortRise 5** (>= 5.3.3). The FortRise 4 version (`tf-mod-fortrise-variant-moveordie`) is no longer maintained: fixes and new features only land in this repository.

## Installation

1. Install FortRise 5 and start the game through `FortRise.exe`.
2. Copy `release/moveordie` (or the shipped folder) into `<TowerFall>/FortRise/Mods/`.

Settings are under **Options > Mods > MoveOrDie**.
Data and log files live in `<TowerFall>/FortRise/Saves/MoveOrDie/` and `<TowerFall>/FortRise/Logs/`.

## Usage

Tick the **MoveOrDie** variant on the versus variants screen, or turn on the
"Pickup activated even when variant is not selected" setting.

> All my mods declare the same `Header` (`EBE1 MODS`), so their variants are
> grouped into a **single column** of the variants screen instead of one column
> per mod.

## Settings

| Setting | Purpose |
|---------|---------|
| Pickup activated even when variant is not selected | apply the effect even when the variant is unticked |
| StationaryDeathTime | how long you may stand still before dying |
| MinSpeed | speed below which you count as standing still |

## Build / deployment

| Script | Purpose |
|--------|---------|
| `script/release.bat` | build, then assemble into `release/` |
| `script/deploy.bat` | copy `release/` into the TowerFall `Mods` folder |
| `script/release_deploy.bat` | both, one after the other |

Paths (game folder, module name) are set in `script/config.bat`.
