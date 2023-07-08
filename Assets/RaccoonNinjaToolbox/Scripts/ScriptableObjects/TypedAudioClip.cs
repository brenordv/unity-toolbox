using System;
using RaccoonNinjaToolbox.Scripts.Attributes;
using RaccoonNinjaToolbox.Scripts.Constants;
using RaccoonNinjaToolbox.Scripts.DataTypes;
using RaccoonNinjaToolbox.Scripts.GlobalControllers;
using UnityEngine;

namespace RaccoonNinjaToolbox.Scripts.ScriptableObjects
{
    [CreateAssetMenu(menuName = ScriptableObjectsPath.TypedAudioPath)]
    public class TypedAudioClip : ScriptableObject
    {
        [SerializeField] private AudioClip audioClip;

        [field: SerializeField, Min(0f),
         Tooltip("Even though the audio can be longer, this property dictates how long until we consider it done. " +
                 "Example: The sound of a door opening may be 10 seconds because of the reverberation and echo, but " +
                 "the actual opening sound might be only 2 seconds. In this case, that's what we put in this " +
                 "property. To use the full duration of the clip, use value zero.")]
        public float practicalDuration { get; private set; }

        [SerializeField] private RangedFloat volume;

        [SerializeField, MinMaxFloatRange(-3f, 3f)]
        private RangedFloat pitch;

        [SerializeField] private bool randomizePitch = true;
        [SerializeField] private bool randomizeVolume = true;

        private void Awake()
        {
            if (audioClip == null)
            {
                Debug.LogError($"{name} requires an AudioClip, but none was found.");
                return;
            }

            if (practicalDuration > 0f) return;
            practicalDuration = audioClip.length;
        }

        public void Play(AudioSource audioSource, Action onFinishCallback = null)
        {
            audioSource.volume = randomizeVolume ? volume.Random() : volume.MinValue;
            audioSource.pitch = randomizePitch ? pitch.Random() : pitch.MinValue;

            if (CanExecuteCallback(onFinishCallback))
                CallbackRunner.Instance.StartCoroutineAfterDelay(practicalDuration, onFinishCallback);
            
            audioSource.PlayOneShot(audioClip);
        }

        private static bool CanExecuteCallback(Action onFinishCallback)
        {
            if (onFinishCallback == null)
                return false;
            
            var runnerExist = CallbackRunner.Instance != null;
            
            if (runnerExist) return true;
            Debug.LogError(
                $"{nameof(CallbackRunner)} is null. Did you forget to add the {nameof(CallbackRunner)} singleton/prefab to the scene?");
            return false;
        }
    }
}