# MoveOrDie

<img width="640" height="400" alt="image" src="https://github.com/user-attachments/assets/d6028bb6-656d-4e3d-8525-c9b965bb22be" />

<img width="640" height="400" alt="headhunters_231210_round_00" src="https://github.com/user-attachments/assets/77f3f114-a6cc-4c9e-8055-85a690949448" />


A "move or die" variant: a player who stands still for too long dies. Both the
delay and the minimum speed that counts as moving are adjustable.

A mod for **FortRise 5** (>= 5.3.3). The FortRise 4 version (`tf-mod-fortrise-variant-moveordie`) is no longer maintained: fixes and new features only land in this repository.

## Installation

1. Install FortRise 5 and start the game through `FortRise.exe`.
2. Copy `release/moveordie` (or the shipped folder) into `<TowerFall>/FortRise/Mods/`.

Settings are under **Options > Mods > MoveOrDie**.
Data and log files live in `<TowerFall>/FortRise/Saves/MoveOrDie/` and `<TowerFall>/FortRise/Logs/`.

## Usage

Three doors onto the same rule, and picking one is all there is to it:

| Door | When to use it |
|---|---|
| The **MoveOrDie variant**, on the versus variants screen | add the rule to an ordinary match |
| The **MOVE OR DIE game mode**, in the versus mode list | make it the subject of the evening |
| The `Pickup activated even when variant is not selected` setting | leave it on permanently, whatever is ticked |

The mode lives in **this** mod rather than a mod of its own. The rule is forty lines that
already exist here; a separate mod would have had two ways to satisfy it — copy those
forty lines, which would start diverging at the first edit, or ask this one to switch on
over interop, which is a lot of plumbing for one boolean. A mode and a variant are not two
features, they are two doors onto the same one.

The mode has **no round logic of its own**: the rule kills archers and the last one
standing wins, which is exactly what `LastManStandingRoundLogic` already does. Writing one
would only have added places to get it wrong.

Press the **left upper shoulder** (Alt2) on that variant to open the mod's settings
right there, without leaving the variants screen. Whatever you change is written to
disk when the window closes.

> All my mods declare the same `Header` (`EBE1 MODS`), so their variants are
> grouped into a **single column** of the variants screen instead of one column
> per mod.

<img width="675" height="317" alt="image" src="https://github.com/user-attachments/assets/2456c746-5446-49f7-977d-25cc6a70d9f9" />

<img width="627" height="237" alt="image" src="https://github.com/user-attachments/assets/e4cd3b6b-7bf1-4510-bac2-8d2426651804" />

<img width="819" height="471" alt="image" src="https://github.com/user-attachments/assets/b51e222e-b157-4cf4-afd2-b459c507f192" />

## What counts as standing still

The timer runs while your speed sits under the threshold, and resets the moment it does
not. **Halfway to the deadline the archer starts flashing** — the warning has to arrive
while there is still time to act on it, not as an epitaph. Running out kills by
`DeathCause.Curse`, and the timer restarts.

Three states are exempt, and each for its own reason: `Frozen`, so the pre-round countdown
does not kill anybody before the round has begun; `Dying`, so a death already under way is
not counted twice; and `LedgeGrab`, because hanging off a ledge is a hold the game itself
offers — punishing it would be punishing a move rather than a refusal to move.

## Settings

| Setting | Purpose |
|---------|---------|
| Pickup activated even when variant is not selected | apply the effect even when the variant is unticked |
| StationaryDeathTime | seconds you may stand still before dying (1-5) |
| MinSpeed | speed below which you count as standing still (1-5) |

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
