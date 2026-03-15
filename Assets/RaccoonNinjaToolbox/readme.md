# Raccoon Ninja's Toolbox

A collection of helper utilities for Unity 6+ to save you from re-implementing the same things every project.

## Features
- **Singleton** — Generic `BaseSingletonController<T>` base class with `PostAwake` hook.
- **RangedInt / RangedFloat** — Min/max values with inspector sliders and `Random()`.
- **TypedAudioClip** — ScriptableObject with randomized volume/pitch and completion callbacks.
- **CallbackRunner** — Global coroutine manager with delay, cancellation, and lifecycle events.
- **InspectorReadOnly** — Attribute to make inspector fields read-only.
- **TagSelector** — Attribute for tag dropdown selection in the inspector.

## Quick Start
```csharp
// Singleton
public class MyManager : BaseSingletonController<MyManager> { }

// Ranged values
[SerializeField, MinMaxIntRange(0, 100)] private RangedInt damage;
int roll = damage.Random();

// Typed audio
[SerializeField] private TypedAudioClip hitSound;
hitSound.Play(audioSource);
```

## Requirements
Unity 6 (6000.0) or later. No additional dependencies.

## Full Documentation
For detailed usage, examples, and development notes, see the
[full readme](https://github.com/brenordv/unity-toolbox/blob/master/readme.md).

## License
MIT — see [full license text](https://github.com/brenordv/unity-toolbox/blob/master/readme.md#license).
