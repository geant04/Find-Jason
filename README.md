# CIS 5660 Final Project: Find Jason!
![findJasonDemoCompressed](https://github.com/user-attachments/assets/93c35d57-3be4-4591-bb53-8a82488f5032)

This is my repository detailing my progress and work creating Find Jason, a challenging surveillance-type game featuring many fun procedural elements!
Features implemented include:
- A state-machine based AI (designed and implemented tasks for NPCs to do in a queue)
- NavMesh Agent navigation, being the pathfinding for all of the AI
- Procedural behavior generation, dependent on player interactions with the environment (running, walking, idle state)
- Procedural crowd placement and NPC decoration! All agents are mathematically unique, sporting different hats, colors, and physical attributes.
- Swivel camera, accessible to keyboard-only and mouse gameplay interaction
- Responsive UI to spotting our target Jason using Unity Shadergraph

An overall final presentation/write-up can be found here: 
https://docs.google.com/presentation/d/1MQsszsQCy6auEcamrufvkKPlD4Cv18ylgz1tqqzATz0/edit?usp=sharing

Thanks for checking the project out!

## Project planning: Design Doc 
https://docs.google.com/document/d/18ytXKak0JDi7xGlXHXz8lmfBFz817IacCAYT6OszzlM/edit?usp=sharing

## Milestone 1: Implementation part 1
- Things did:
  - Implemented and designed a NPC AI system, included with a task-system (idle, walk using NavMesh)
  - Created procedural crowd placement
  - Basic camera controller movement
  - Added a simple decorated map
  - Game includes UI (timer for how long it took to find Jason) + shader for camera crosshair

- Screenshots:
  - ![Screenshot 2024-11-10 013112](https://github.com/user-attachments/assets/c5f5e464-7966-4cad-8655-68f33d731656)
  - This is like the only screenshot I took testing NavMesh behavior, I can go back to my commit history and grab some early screenshots later


## Milestone 2: Implementation part 2
- Things did: 
  - NPCs are now decorated with fun colors and random hats (fedora, cap, wizard hat, top hat)
  - Gameplay tweaks based on testing and design feedback added, for ex: crosshair has more vibrant colors
  - More procedural-esque elements will be added to the NPCs

- Screenshots:
  - ![Screenshot 2024-11-24 183425](https://github.com/user-attachments/assets/d4e9361e-c80b-49f0-9b5b-3341dc495dd1)

## Final submission
- Final results: essentially above, changed NPCs but visually not quite noticeable at first glance
- Post Mortem: https://docs.google.com/presentation/d/1MQsszsQCy6auEcamrufvkKPlD4Cv18ylgz1tqqzATz0/edit?usp=sharing
