# AGENTS.md

## Project Facts
- Unity project targeting editor `2022.3.62f2c1`; open the repository root as the Unity project.
- Render pipeline is URP 14 (`com.unity.render-pipelines.universal` 14.0.12).
- Enabled build scenes are `Assets/Scenes/Persistent.unity` then `Assets/Scenes/Playground.unity`; treat `Persistent` as the startup/bootstrap scene unless scene setup proves otherwise.
- The Unity package lock uses the China registry (`https://packages.unity.cn`) for registry packages.

## Code Map
- Project gameplay scripts live under `Assets/Scripts`; large imported/vendor assets live under `Assets/Behavior Designer`, `Assets/Plugins`, `Assets/MySetting/MagicaCloth2`, `Assets/MySetting/MMD4Mecanim`, and most of `Assets/Art`.
- Player input is Unity Input System based: `Assets/MySetting/PlayerControl.inputactions` is the source asset and `Assets/MySetting/PlayerControl.cs` is generated code. Do not hand-edit the generated `.cs` file.
- `GameInputManager` wraps `PlayerInput` actions and exposes the current input state; `TeamInputManager` consumes it for attacks and link skills.
- Team switching is centralized in `TeamManager`: it discovers team members from its child transforms, toggles `Player`/`Companion` tags, switches `PlayerMovementControl`, `CompanionAI`, `CompanionMovementAgent`, `BehaviorTree`, and `NavMeshAgent`, then broadcasts `Events.SwitchMainCharacter`.
- Combat state machines derive from `CombatStateMachineBase`; subclasses add character-specific `CombatStateType` states, while transitions are gated by the priority table in the base class.
- Cross-system notifications use `EventCenter` with message classes in `Assets/Scripts/Events/Events.cs`; listeners should be paired in `OnEnable`/`OnDisable`.

## Verification
- No repo-local CI, test runner script, or lint command is present. Prefer opening Unity and checking Console errors after script changes.
- If Unity is available in automation, a focused compile/test check should use batchmode with this project path, e.g. `Unity.exe -batchmode -quit -projectPath . -runTests -testPlatform EditMode`; confirm the local Unity executable path first.
- `.sln` and `.csproj` files are Unity-generated and ignored by git; do not treat changes to them as source changes.

## Editing Cautions
- Preserve `.meta` files when moving or renaming Unity assets.
- Avoid editing imported third-party packages unless the task is explicitly about them.
- This project mixes English identifiers with Chinese comments; keep existing style local to the file and avoid broad formatting-only rewrites.
