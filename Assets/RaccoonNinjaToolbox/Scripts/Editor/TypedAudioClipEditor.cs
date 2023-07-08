using RaccoonNinjaToolbox.Scripts.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace RaccoonNinjaToolbox.Scripts.Editor
{
    [CustomEditor(typeof(TypedAudioClip))]
    public class TypedAudioClipEditor : UnityEditor.Editor
    {
        private static AudioSource _audioSrc;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var typedAudioClip = (TypedAudioClip)target;

            if (!HasAudioSource())
            {
                GUILayout.Label("No AudioSource found on the main camera. If you want to preview audio clips, please add an AudioSource to the main camera.");
                return;
            }
            
            if (!GUILayout.Button("Play Clip")) return;
            
            typedAudioClip.Play(_audioSrc);
        }
        
        private static bool HasAudioSource()
        {
            GetAudioSource();
            return _audioSrc != null;
        }

        private static void GetAudioSource()
        {
            if (_audioSrc != null) return;

            _audioSrc = FindObjectOfType<AudioSource>();

            if (_audioSrc == null)
            {
                Debug.LogError(
                    "No AudioSource found on the main camera. If you want to preview audio clips, please add an AudioSource to the main camera.");
                return;
            }

            _audioSrc.playOnAwake = false;
        }
    }
}