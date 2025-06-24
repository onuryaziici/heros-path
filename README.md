# Hero's Path - A Simple Action RPG Prototype

![Gameplay Showcase](./.github/images/herospath-gif.gif)

A simple yet comprehensive top-down Action RPG prototype developed in Unity. This project was created as a portfolio piece to demonstrate core game development skills, including C# programming, system design, and Unity engine proficiency.

**[► Watch Gameplay Video](https://www.youtube.com/watch?v=4RdXq3yN5FY) ** 

---

## 🌟 Core Features

This prototype was built based on a Game Design Document (GDD) and includes the following key systems:

*   **Advanced Camera System:**
    *   Full 360-degree orbit camera controlled by the mouse.
    *   Dynamic zoom functionality with the scroll wheel.
    *   Robust camera collision detection to prevent clipping through objects.
    *   Context-aware cursor management (locked during gameplay, unlocked for UI interaction).

*   **Player & Character Controller:**
    *   Camera-relative movement controls.
    *   Manual gravity implementation to handle slopes and uneven terrain.
    *   Fully animated character with states for idle, run, attack, hurt, and death.
    *   Animation-driven combat system with "Animation Commitment" and "Hit Stun" mechanics for impactful gameplay feel.

*   **Combat System:**
    *   Real-time, hitbox-based melee combat.
    *   **Animation Events** are used for precise damage timing, synchronizing visuals with gameplay logic.
    *   A system to prevent multi-hit bugs on a single attack swing.

*   **Enemy AI & Health System:**
    *   Basic AI using Unity's NavMeshAgent for player detection and pursuit.
    *   Enemies have their own health, attack, and "Hit Stun" states.
    *   Visual feedback for health via world-space UI health bars.
    *   Visual effects (VFX) for taking damage (flash) and death.

*   **Inventory & Equipment System:**
    *   Data-driven inventory system using **ScriptableObjects** for creating and managing items (potions, weapons).
    *   A fully functional UI with a grid layout, item stacking, and interaction (use/equip).
    *   Weapon equipping system that dynamically affects player stats (e.g., `totalDamage`).

*   **Loot & Interaction System:**
    *   Enemies can drop items from a loot table upon death.
    *   A player interaction system allows picking up items from the world with a key press (`E`).
    *   Contextual UI prompts to notify the player of nearby interactable items.

*   **Audio & Environment:**
    *   A singleton `AudioManager` to handle both SFX and background music.
    *   Sound effects for key actions (attacks, hits, UI clicks, etc.).
    *   A basic environment built with Unity's Terrain tools, featuring trees, grass, and optimized details.

---

## 🛠️ Built With

*   **Engine:** Unity 6000.0.50f1
*   **Language:** C#
*   **Render Pipeline:** Universal Render Pipeline (URP)
*   **Modeling/Animation:** Mixamo for rigging and animations. Hunyuan3D for 3D modelling

---

## 🚀 Getting Started

To run this project locally:

1.  Clone the repository: `git clone https://github.com/onuryaziici/heros-path.git`
2.  Open the project in Unity Hub with the correct Unity version (6000.0.50f1).
3.  Open the `GameplayLevel` scene located in the `_Scenes` folder.
4.  Press Play!

---

## 🎬 Showcase

### Dynamic Camera in Action
![Camera Orbit and Collision](./.github/images/camera-gif.gif)
