using System.Reflection;
using RaccoonNinjaToolbox.Scripts.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace RaccoonNinjaToolbox.Scripts.Editor
{
    [CustomEditor(typeof(TypedAudioClip))]
    public class TypedAudioClipEditor : UnityEditor.Editor
    {
        private static MethodInfo _playClipMethod;
        private static MethodInfo _stopClipsMethod;
        private static AudioSource _previewSource;
        private AudioClip _previewClip;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var clipProp = serializedObject.FindProperty("audioClip");
            if (!clipProp.objectReferenceValue)
            {
                GUILayout.Label("Assign an AudioClip to enable preview.");
                return;
            }

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Play Clip"))
                PlayPreview();

            if (GUILayout.Button("Stop"))
                StopPreview();

            GUILayout.EndHorizontal();
        }

        private void PlayPreview()
        {
            StopPreview();
            CleanupPreviewClip();

            var source = GetOrCreatePreviewSource();
            var typedAudioClip = (TypedAudioClip)target;

            // Call Play() to exercise the actual runtime randomization logic.
            // PlayOneShot won't produce audio in edit mode, but the volume/pitch
            // values are applied to the AudioSource and we capture them below.
            typedAudioClip.Play(source);

            var clip = serializedObject.FindProperty("audioClip").objectReferenceValue as AudioClip;
            if (!clip) return;

            float volume = source.volume;
            float pitch = Mathf.Max(source.pitch, 0.01f);

            _previewClip = CreateModifiedClip(clip, volume, pitch);
            if (_previewClip)
                PlayClipViaAudioUtil(_previewClip);
        }

        private static AudioClip CreateModifiedClip(AudioClip source, float volume, float pitch)
        {
            var samples = new float[source.samples * source.channels];
            source.GetData(samples, 0);

            for (var i = 0; i < samples.Length; i++)
                samples[i] *= volume;

            // Changing the sample rate shifts pitch without resampling:
            // higher rate = faster playback = higher pitch.
            var modifiedRate = Mathf.RoundToInt(source.frequency * pitch);
            var clip = AudioClip.Create(
                "TypedAudioClip_Preview",
                source.samples,
                source.channels,
                modifiedRate,
                false);
            clip.SetData(samples, 0);
            return clip;
        }

        private void OnDisable()
        {
            StopPreview();
            CleanupPreviewClip();
            if (!_previewSource) return;
            DestroyImmediate(_previewSource.gameObject);
            _previewSource = null;
        }

        private void CleanupPreviewClip()
        {
            if (!_previewClip) return;
            DestroyImmediate(_previewClip);
            _previewClip = null;
        }

        private static AudioSource GetOrCreatePreviewSource()
        {
            if (_previewSource) return _previewSource;

            var go = new GameObject("TypedAudioClip Preview")
            {
                hideFlags = HideFlags.HideAndDontSave
            };
            _previewSource = go.AddComponent<AudioSource>();
            _previewSource.playOnAwake = false;
            return _previewSource;
        }

        private static void PlayClipViaAudioUtil(AudioClip clip)
        {
            EnsureReflectionCache();
            _playClipMethod?.Invoke(null, new object[] { clip, 0, false });
        }

        private static void StopPreview()
        {
            EnsureReflectionCache();
            _stopClipsMethod?.Invoke(null, null);
        }

        private static void EnsureReflectionCache()
        {
            if (_playClipMethod != null) return;

            var audioUtilType = typeof(AudioImporter).Assembly.GetType("UnityEditor.AudioUtil");
            if (audioUtilType == null) return;

            _playClipMethod = audioUtilType.GetMethod(
                "PlayPreviewClip",
                BindingFlags.Static | BindingFlags.Public,
                null,
                new[] { typeof(AudioClip), typeof(int), typeof(bool) },
                null);

            _stopClipsMethod = audioUtilType.GetMethod(
                "StopAllPreviewClips",
                BindingFlags.Static | BindingFlags.Public);
        }
    }
}
