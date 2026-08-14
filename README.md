# MoveOrDie

<img width="640" height="480" alt="headhunters_231210_round_00" src="https://github.com/user-attachments/assets/77f3f114-a6cc-4c9e-8055-85a690949448" />


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

Press the **left upper shoulder** (Alt2) on that variant to open the mod's settings
right there, without leaving the variants screen. Whatever you change is written to
disk when the window closes.

> All my mods declare the same `Header` (`EBE1 MODS`), so their variants are
> grouped into a **single column** of the variants screen instead of one column
> per mod.

<img width="675" height="317" alt="image" src="https://github.com/user-attachments/assets/2456c746-5446-49f7-977d-25cc6a70d9f9" />

<img width="627" height="237" alt="image" src="https://github.com/user-attachments/assets/e4cd3b6b-7bf1-4510-bac2-8d2426651804" />

<img width="819" height="471" alt="image" src="https://github.com/user-attachments/assets/b51e222e-b157-4cf4-afd2-b459c507f192" />

## Settings

| Setting | Purpose |
|---------|---------|
| Pickup activated even when variant is not selected | apply the effect even when the variant is unticked |
| StationaryDeathTime | how long you may stand still before dying |
| MinSpeed | speed below which you count as standing still |

## Settings from the variant screen

Every setting below is also reachable **without leaving the versus screen**: highlight
this mod's variant box and press the **left upper shoulder** (`Alt2`). A small window
opens on the settings that matter, and the button is announced in the guide at the
bottom.

Changes are written to disk immediately. FortRise only saves settings when leaving its
own Options menu, so a value changed here - or right before quitting - used to be lost.

> The window recognises its own box by the label the game *displays*, ignoring case and
> spaces. Comparing it letter for letter with the registered name never matched: the game
> shows variant titles in **capitals** (`BLACKHOLE` for `BlackHole`), and the failure was
> completely silent - no sound, no message, nothing.
>
> It is also re-anchored on the camera every frame. Menu entities live on a layer that
> **scrolls**, so a window placed at a fixed position stayed where the list was when it
> opened - drawn, but above the visible area as soon as you had scrolled down.

## Build / deployment

| Script | Purpose |
|--------|---------|
| `script/release.bat` | build, then assemble into `release/` |
| `script/deploy.bat` | copy `release/` into the TowerFall `Mods` folder |
| `script/release_deploy.bat` | both, one after the other |

Paths (game folder, module name) are set in `script/config.bat`.
