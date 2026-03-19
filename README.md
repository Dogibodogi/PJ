# I Beg You, Do Nothing (Moon Edition) 🚀🌕

A puzzle-exploration game inspired by *"Please, Don't Touch Anything"*. You are seated at the control desk of a highly advanced space rocket. You are explicitly instructed to do nothing... but curiosity always wins. Every button, switch, and tool on the desk can trigger chain reactions, reveal hidden secrets, or lead to various absurd, catastrophic, or mysterious endings.

---

## 🛠️ How to Set Up the Project

### 1. Prerequisites
* Download and install **Unity Hub**.
* Install a recent version of the **Unity Editor** (2021.3 LTS or newer recommended).
* Make sure you have **Git** installed on your computer.

### 2. Creating the Project
If you are starting fresh or recreating the environment:
1. Open Unity Hub and click **New Project**.
2. Select the **2D URP (Universal Render Pipeline)** template (this ensures the lighting, 2D sprites, and UI look modern and performant).
3. Name the project `I Beg You, Do Nothing (Moon Edition)` and click **Create Project**.
4. Once inside Unity, ensure you have the **TextMeshPro** package installed and imported (Window > TextMeshPro > Import TMP Essential Resources).

---

## 🔗 How to Connect to GitHub

To link your local Unity project to a GitHub repository, open your terminal (Command Prompt, Git Bash, or VS Code Terminal) inside the root folder of your Unity project and run the following commands:
```bash
# 1. Initialize the local directory as a Git repository
git init

# (Important: Make sure you have a Unity .gitignore file in the root folder 
# to avoid uploading massive temporary files like the Library folder!)

# 2. Add all project files to the staging area
git add .

# 3. Commit the changes
git commit -m "Initial commit: Base project setup and UI systems"

# 4. Rename the default branch to 'main'
git branch -M main

# 5. Link to your remote GitHub repository (Replace the URL with your actual repo link)
git remote add origin https://github.com/Dogibodogi/PJ

# 6. Push the code to GitHub
git push -u origin main
```

---

## ⚙️ What Has Been Implemented

The game currently features a modular, Object-Oriented system for desk interactions and puzzle-solving.

### Core Systems & Classes

**`InteractableTool.cs`** — An abstract base class using `IPointerClickHandler`. All interactive objects on the desk inherit from this, keeping the codebase clean and modular.

**`DeskController.cs`** — Manages global desk events and environmental reactions (e.g., turning the desk red when an alert is triggered).

### 🔦 The UV Light System (Event-Driven)

A global event system that reveals invisible clues without tightly coupling objects.

**`UVLightTool.cs`** — The interactive UV stick. When clicked, it toggles its state and broadcasts an `OnUVLightStateChanged` event to the entire game.

**`HiddenSecret.cs`** — Attached to fingerprints, hidden text, and invisible clues. These objects remain completely invisible (alpha = 0) until they "hear" the UV light turn on, at which point they reveal themselves (alpha = 1).

### ⌨️ The Numpad & LED Screen

A fully functional digital input system for triggering endings.

**`NumpadController.cs`** — The "brain" of the keyboard. It handles the logic for the TextMeshPro LED screen, stores the player's input, and checks it against secret codes (e.g., `1234`, `0000`, `6666`) to trigger specific game events.

**`NumpadButton.cs`** — Attached to individual UI buttons (0–9, CLEAR, ENTER). They automatically find the `NumpadController` and send their specific string values when clicked.

**UI Layout** — The buttons are perfectly aligned using Unity's **Grid Layout Group** component for a realistic keypad look.

---

## 💡 Future Ideas & Roadmap (To Be Implemented)

To expand the "Don't Touch Anything" vibe, the following interactive objects and puzzles are planned for the desk:

### Anti-Gravity Switch 🪐
A switch under a safety cover. Turning it off causes small desk items (coffee cup, pens) to float. Letting them float too long causes a hull breach (Ending).
> **Secret:** A PIN code is taped to the bottom of the floating coffee cup.

### Space Coffee Cup ☕
Clicking it repeatedly spills coffee on the console, short-circuiting a panel to reveal a wire-cutting mini-puzzle.

### Radio Frequency Tuner 📻
A dial to change frequencies. Finding the right static channel reveals a Morse code audio clue, or intercepts an alien broadcast leading to a specific ending.

### Blast Shield Lever 🪟
A heavy lever that opens the metal window shutters in front of the desk. Depending on the codes entered in the Numpad, the window might reveal Earth, the Moon, or a rapidly approaching asteroid.

### O2 / Life Support Valve 💨
Turning down the oxygen dims the screen and creates a red pulsating vignette. In this "hallucination" state, certain buttons change symbols, revealing alien text that is normally invisible.

### The Pilot's Manual & Floppy Disk 💾
An interactive book on the desk. Flipping to the center reveals a hollowed-out section containing a Floppy Disk. Inserting this disk into a slot overrides the Numpad, allowing the launch of nuclear missiles.