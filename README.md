# Soccer Card Pack & Squad Builder

## Overview
This Unity project is a simple soccer card game where users open a pack of cards, build a squad, and play a simulated match against AI. The project demonstrates UI, animation, networking, and addressables usage in Unity.

## Features
- Animated pack opening with 5 random soccer cards.
- Drag & drop squad builder: select 3 cards for your team.
- Match simulation against AI with animated results.
- All card/player data loaded from a JSON file (local or remote).
- Dynamic loading of UI and player models using Unity Addressables.

## Requirements
- Unity 2021 or higher
- .NET Framework 4.7.1
- Packages:
  - Cysharp UniTask (async/await for Unity)
  - TextMeshPro (for UI text)
  - Unity Addressables (for asset management)

> All packages can be installed via Unity Package Manager.

## Setup Instructions

1. **Clone or unzip the project folder.**
2. **Open the project in Unity.**
3. **Install required packages:**
   - Open Window > Package Manager.
   - Search for and install:
     - Addressables
     - TextMeshPro
     - UniTask (add via Git URL: `https://github.com/Cysharp/UniTask.git`)
4. **Build Addressables:**
   - Window > Asset Management > Addressables > Build > New Build > Default Build Script.
5. **Add card data JSON:**
   - Place your `cards.json` file in `Assets/StreamingAssets/`.
   - Example format:
```json
[
  {
    "id": "player001",
    "name": "Lionel Messi",
    "rating": 94,
    "thumbnail": "https://example.com/messi.png",
    "addressableModel": "MessiModel"
  }
]
```
   - You can use a local file or host it online (update the endpoint in `CardSystemController.cs` if needed).

## How to Run

1. **Open the main scene in Unity.**
2. **Press Play.**
3. **Game Flow:**
   - Start at the main menu.
   - Click "Open Pack" to reveal 5 random cards.
   - Click "Next" to go to squad builder.
   - Drag and drop 3 cards into squad slots.
   - Click "Play Match" to simulate a match against AI.
   - View animated results (Victory/Lose/Draw).
   - Click "Play Again" to restart the game.

## Project Structure
- **Core:** Game logic, services, and controllers.
- **UI System:** Views, CardView, SquadSlotView, and UI prefabs.
- **Card System:** Card models, repository, and networking.
- **Bootstrap System:** Startup and service registration.

## Customization
- **CardPackOpenAnimationConfig:** Animation parameters for pack opening are stored in a ScriptableObject and loaded via Addressables.
- **Player models:** Should be set up as Addressables and referenced in the JSON.

## Troubleshooting
- If Addressables do not load, rebuild Addressables and check asset labels.
- If JSON is not loading, verify the file path and format.
- Make sure all required packages are installed and up to date.

