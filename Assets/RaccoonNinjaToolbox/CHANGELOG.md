# Changelog

## [3.0.0] - 2026-03-15

### Added
- CancellationToken support in TypedAudioClip.Play for caller-controlled cancellation.
- TypedAudioClipDemo script showcasing play counter and completion callback.

### Changed
- **Breaking:** TypedAudioClip no longer depends on CallbackRunner for completion callbacks. Uses Unity 6's Awaitable for fire-and-forget async delays instead.
- **Breaking:** TypedAudioClip no longer modifies volume/pitch when randomization is disabled. The AudioSource retains its existing values.
- TypedAudioClip.Play method signature now includes an optional CancellationToken parameter.

### Fixed
- Floating-point precision in RangedFloat tests (added tolerance for min-equals-max edge case).

## [2.0.0] - 2026-03-15

### Added
- Unit tests for all core components (Edit Mode and Play Mode).

### Changed
- Minimum Unity version raised to Unity 6 (6000.0).
- Removed `com.unity.textmeshpro` package dependency (merged into `com.unity.ugui` in Unity 6).
- Rewrote TypedAudioClipEditor to use a dedicated `TypedAudioClipPreview` utility, replacing the old `FindObjectOfType`-based approach.

### Fixed
- BaseSingletonController now resets static instance on domain reload via `[RuntimeInitializeOnLoadMethod]`, fixing Enter Play Mode Options with domain reload disabled.
- BaseSingletonController now nulls the instance on `OnDestroy`, preventing stale references.
- CallbackRunner no longer throws `ArgumentException` on delayed coroutines due to duplicate dictionary key.
- CallbackRunner debug list (`runningCoroutineKeys`) no longer accumulates duplicate entries for delayed coroutines.
- CallbackRunner `onCoroutineStarted` event is now null-safe, consistent with the other events.
- TypedAudioClip `practicalDuration` fallback moved from `Awake` to `Play` for reliable runtime behavior.
- TypedAudioClip `Play` now guards against null `audioClip`.

## [1.0.0] - 2023-07-01

### Added
- BaseSingletonController generic singleton base class.
- CallbackRunner coroutine manager singleton.
- TypedAudioClip ScriptableObject with randomized volume/pitch.
- RangedFloat and RangedInt data types with inspector sliders.
- InspectorReadOnly attribute.
- TagSelector attribute.
