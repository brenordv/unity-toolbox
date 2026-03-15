using RaccoonNinjaToolbox.Scripts.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace RaccoonNinjaToolbox.Scripts.Editor
{
    [CustomEditor(typeof(TypedAudioClip))]
    public class TypedAudioClipEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var typedAudioClip = (TypedAudioClip)target;

            if (!typedAudioClip.AudioClip)
            {
                GUILayout.Label("Assign an AudioClip to enable preview.");
                return;
            }

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Play Clip"))
                TypedAudioClipPreview.PlayPreview(typedAudioClip);

            if (GUILayout.Button("Stop"))
                TypedAudioClipPreview.StopPreview();

            GUILayout.EndHorizontal();
        }

        private void OnDisable()
        {
            TypedAudioClipPreview.Cleanup();
        }
    }
}
