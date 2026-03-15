using System.Reflection;
using RaccoonNinjaToolbox.Scripts.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace RaccoonNinjaToolbox.Scripts.Editor
{
    internal static class TypedAudioClipPreview
    {
        private static MethodInfo _playClipMethod;
        private static MethodInfo _stopClipsMethod;
        private static AudioSource _previewSource;
        private static AudioClip _previewClip;
        private static readonly object[] PlayArgs = { null, 0, false };

        [InitializeOnLoadMethod]
        private static void OnDomainReload() => Cleanup();

        internal static void PlayPreview(TypedAudioClip typedClip)
        {
            if (!typedClip) return;

            StopPreview();
            CleanupPreviewClip();

            var source = GetOrCreatePreviewSource();
            typedClip.Play(source);

            var clip = typedClip.AudioClip;
            if (!clip) return;

            var volume = source.volume;
            var pitch = Mathf.Max(source.pitch, 0.01f);

            _previewClip = CreateModifiedClip(clip, volume, pitch);
            if (_previewClip)
                PlayClipViaAudioUtil(_previewClip);
        }

        internal static void StopPreview()
        {
            EnsureReflectionCache();
            _stopClipsMethod?.Invoke(null, null);
        }

        internal static void Cleanup()
        {
            StopPreview();
            CleanupPreviewClip();
            if (!_previewSource) return;
            Object.DestroyImmediate(_previewSource.gameObject);
            _previewSource = null;
        }

        private static AudioClip CreateModifiedClip(AudioClip source, float volume, float pitch)
        {
            var samples = new float[source.samples * source.channels];
            source.GetData(samples, 0);

            for (var i = 0; i < samples.Length; i++)
                samples[i] *= volume;

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

        private static void CleanupPreviewClip()
        {
            if (!_previewClip) return;
            Object.DestroyImmediate(_previewClip);
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
            PlayArgs[0] = clip;
            _playClipMethod?.Invoke(null, PlayArgs);
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
