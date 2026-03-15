using System;
using System.Threading;
using RaccoonNinjaToolbox.Scripts.Attributes;
using RaccoonNinjaToolbox.Scripts.Constants;
using RaccoonNinjaToolbox.Scripts.DataTypes;
using UnityEngine;
using UnityEngine.Serialization;

namespace RaccoonNinjaToolbox.Scripts.ScriptableObjects
{
    [CreateAssetMenu(menuName = ScriptableObjectsPath.TypedAudioPath)]
    public class TypedAudioClip : ScriptableObject
    {
        [SerializeField] private AudioClip audioClip;
        public AudioClip AudioClip => audioClip;

        [field: SerializeField, FormerlySerializedAs("<practicalDuration>k__BackingField"), Min(0f),
                Tooltip("Even though the audio can be longer, this property dictates how long until we consider it done. " +
                        "Example: The sound of a door opening may be 10 seconds because of the reverberation and echo, but " +
                        "the actual opening sound might be only 2 seconds. In this case, that's what we put in this " +
                        "property. To use the full duration of the clip, use value zero.")]
        public float PracticalDuration { get; private set; }

        [SerializeField] private RangedFloat volume;

        [SerializeField, MinMaxFloatRange(-3f, 3f)]
        private RangedFloat pitch;

        [SerializeField, Tooltip("If set, will randomize the pitch of the audio clip according to the range defined. " +
                                 "If false, will use whatever is defined in the audio source.")] 
        private bool randomizePitch = true;

        [SerializeField, Tooltip("If set, will randomize the volume of the audio clip according to the range defined. " +
                                 "If false, will use whatever is defined in the audio source.")]
        private bool randomizeVolume = true;

        public void Play(AudioSource audioSource, Action onFinishCallback = null, CancellationToken ct = default)
        {
            if (!CanPlayClip(audioSource)) return;

            ProcessPracticalDuration();

            ProcessClipConfigRandomization(audioSource);

            audioSource.PlayOneShot(audioClip);

            if (onFinishCallback == null) return;

            _ = FireAndForgetCallbackAsync(onFinishCallback, ct);
        }

        private bool CanPlayClip(AudioSource audioSource)
        {
            if (!audioSource)
            {
                Debug.LogError($"{name} requires an AudioSource, but none was provided.");
                return false;
            }

            if (!audioClip)
            {
                Debug.LogError($"{name} requires an AudioClip, but none was found.");
                return false;
            }

            return true;
        }

        private void ProcessPracticalDuration()
        {
            if (PracticalDuration > 0f) return;

            PracticalDuration = audioClip.length;
        }

        private void ProcessClipConfigRandomization(AudioSource audioSource)
        {
            if (randomizeVolume) audioSource.volume = volume.Random();
            if (randomizePitch) audioSource.pitch = pitch.Random();
        }

        private async Awaitable FireAndForgetCallbackAsync(Action onFinishCallback, CancellationToken ct)
        {
            try
            {
                await Awaitable.WaitForSecondsAsync(PracticalDuration, ct);
                onFinishCallback();
            }
            catch (OperationCanceledException)
            {
                // In this specific case, no need to do anything here, the operation was canceled. So we just move on.
            }
        }
    }
}