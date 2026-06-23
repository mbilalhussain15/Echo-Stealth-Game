# 🤖 Echo Stealth Game

A 2D top-down stealth game built in **Unity** with the **Universal Render Pipeline (URP)** — featuring grid-based movement, enemy AI with BFS pathfinding, a stealth mechanic, hackable security nodes, an energy system, and procedurally generated levels.

---

## 🎮 Gameplay

Navigate a grid-based map, avoid or outsmart security bots, hack nodes to disable enemies, and reach the exit — all while managing your energy.

### Controls

| Key | Action |
|-----|--------|
| `W / ↑` | Move Up |
| `S / ↓` | Move Down |
| `A / ←` | Move Left |
| `D / →` | Move Right |
| `Space` | Activate Stealth (6 seconds) |

---

## ✨ Features

- 🧭 **Grid-based Movement** — tile-by-tile navigation across a procedurally connected grid
- 👻 **Stealth System** — press Space to go invisible for 6 seconds; enemies lose detection and stop chasing (UI countdown timer shown)
- 🤖 **SecurityBot AI** — bots patrol randomly across the grid; on player detection they switch to active BFS pathfinding to chase
- 🔓 **Security Node Hacking** — hack nodes to freeze all SecurityBots simultaneously for 6 seconds via a C# event system
- ⚡ **Energy System** — player energy drains over time; recharge at RechargeStation tiles scattered across the map
- 🚪 **Exit Node** — reach the exit tile to complete the level
- 🗺️ **Procedural Level Generation** — levels are generated dynamically via `LevelGenerator` and `GridManager`
- 📖 **Story / Lore System** — narrative events logged via `StoryData` and `StoryLog`
- 🎬 **Main Menu & Endings** — full game flow with `MainMenuManager` and `EndingManager`

---

## 🧠 AI & Pathfinding

SecurityBots use a custom **Breadth-First Search (BFS)** algorithm (no NavMesh) to find the shortest path to the player on the tile grid:

```
Idle → Patrol (random tile walk)
           ↓ (player enters trigger zone)
       Chase (BFS pathfinding to player)
           ↓ (player activates stealth OR node hacked)
       Patrol / Frozen (6s)
```

- Stealth detection: `Physics2D.IgnoreCollision` toggled based on `PlayerMovement.IsStealthed`
- Node hack event: `SecurityNode.OnNodeHacked` (C# Action event) → all bots subscribe and freeze

---

## 🛠️ Tech Stack

![Unity](https://img.shields.io/badge/Unity-000000?style=for-the-badge&logo=unity&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![URP](https://img.shields.io/badge/URP-Universal_Render_Pipeline-blue?style=for-the-badge)

| Technology | Purpose |
|------------|---------|
| Unity (2D) | Game engine |
| Universal Render Pipeline (URP) | Rendering & custom shaders |
| C# | All game logic & AI |
| Unity Tilemap | Grid-based level layout |
| Unity Input System | Keyboard input handling |
| TextMesh Pro | In-game UI text & timers |
| ShaderLab / HLSL | Custom visual shaders |

---

## 📁 Project Structure

```
Echo-Stealth-Game/
├── Assets/
│   ├── Scripts/
│   │   ├── PlayerMovement.cs     # Grid movement + stealth mechanic
│   │   ├── SecurityBot.cs        # Enemy AI — patrol + BFS pathfinding + freeze
│   │   ├── SecurityNode.cs       # Hackable node — broadcasts freeze event
│   │   ├── EnergySystem.cs       # Player energy drain & management
│   │   ├── RechargeStation.cs    # Energy recharge trigger
│   │   ├── ExitNode.cs           # Level completion trigger
│   │   ├── GridManager.cs        # Grid construction & tile connectivity
│   │   ├── TileNode.cs           # Individual tile data
│   │   ├── LevelGenerator.cs     # Procedural level generation
│   │   ├── GameManager.cs        # Game state (win / game over)
│   │   ├── EndingManager.cs      # Ending sequences
│   │   ├── StoryData.cs          # Narrative data structures
│   │   ├── StoryLog.cs           # In-game story log UI
│   │   └── MainMenuManager.cs    # Main menu logic
│   ├── Scenes/                   # Unity scenes
│   ├── Sprites/                  # 2D sprite assets
│   ├── Tiles/                    # Tilemap tiles
│   ├── Prefabs/                  # Reusable game object prefabs
│   ├── audio/                    # Sound effects & music
│   ├── Images/                   # UI images
│   └── Settings/                 # URP render pipeline settings
```

---

## 🚀 Getting Started

### Prerequisites

- [Unity 2022.3+ LTS](https://unity.com/download) with **Universal Render Pipeline** package
- Windows / macOS

### Run the Project

```bash
# Clone the repository
git clone https://github.com/mbilalhussain15/Echo-Stealth-Game.git
```

1. Open **Unity Hub** → **Open Project** → select the cloned folder
2. Open `Assets/Scenes/` and load the main scene
3. Press **Play** in the Unity Editor

---

## 📄 License

MIT © [Bilal Hussain](https://github.com/mbilalhussain15)
