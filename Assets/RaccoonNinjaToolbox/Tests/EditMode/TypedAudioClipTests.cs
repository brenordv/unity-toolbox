using System.Reflection;
using NUnit.Framework;
using RaccoonNinjaToolbox.Scripts.DataTypes;
using RaccoonNinjaToolbox.Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.TestTools;

namespace RaccoonNinjaToolbox.Tests.EditMode
{
    [TestFixture]
    public class TypedAudioClipTests
    {
        private TypedAudioClip _clip;
        private GameObject _audioGo;
        private AudioSource _audioSource;

        [SetUp]
        public void SetUp()
        {
            _clip = ScriptableObject.CreateInstance<TypedAudioClip>();
            _audioGo = new GameObject("TestAudioSource");
            _audioSource = _audioGo.AddComponent<AudioSource>();
        }

        [TearDown]
        public void TearDown()
        {
            if (_clip) UnityEngine.Object.DestroyImmediate(_clip);
            if (_audioGo) UnityEngine.Object.DestroyImmediate(_audioGo);
        }

        [Test]
        public void Play_WithNoAudioClip_LogsError()
        {
            LogAssert.Expect(LogType.Error, new System.Text.RegularExpressions.Regex("requires an AudioClip"));

            _clip.Play(_audioSource);
        }

        [Test]
        public void Play_WithAudioClip_SetsPracticalDurationToClipLength_WhenZero()
        {
            var audioClip = AudioClip.Create("TestClip", 44100, 1, 44100, false);
            SetPrivateField(_clip, "audioClip", audioClip);
            SetPrivateField(_clip, "randomizeVolume", false);
            SetPrivateField(_clip, "randomizePitch", false);
            SetPrivateField(_clip, "volume", new RangedFloat { MinValue = 0.5f, MaxValue = 1f });
            SetPrivateField(_clip, "pitch", new RangedFloat { MinValue = 1f, MaxValue = 1f });

            Assert.That(_clip.PracticalDuration, Is.EqualTo(0f), "PracticalDuration should start at 0");

            _clip.Play(_audioSource);

            Assert.That(_clip.PracticalDuration, Is.EqualTo(audioClip.length).Within(0.01f),
                "PracticalDuration should be set to clip length when it was zero");

            AudioClip.DestroyImmediate(audioClip);
        }

        [Test]
        public void Play_DoesNotModifyVolume_WhenNotRandomized()
        {
            var audioClip = AudioClip.Create("TestClip", 44100, 1, 44100, false);
            SetPrivateField(_clip, "audioClip", audioClip);
            SetPrivateField(_clip, "randomizeVolume", false);
            SetPrivateField(_clip, "randomizePitch", false);
            SetPrivateField(_clip, "volume", new RangedFloat { MinValue = 0.42f, MaxValue = 0.9f });
            SetPrivateField(_clip, "pitch", new RangedFloat { MinValue = 1f, MaxValue = 1f });

            _audioSource.volume = 0.65f;
            _clip.Play(_audioSource);

            Assert.That(_audioSource.volume, Is.EqualTo(0.65f).Within(0.001f),
                "Volume should remain unchanged when randomization is disabled");

            AudioClip.DestroyImmediate(audioClip);
        }

        [Test]
        public void Play_DoesNotModifyPitch_WhenNotRandomized()
        {
            var audioClip = AudioClip.Create("TestClip", 44100, 1, 44100, false);
            SetPrivateField(_clip, "audioClip", audioClip);
            SetPrivateField(_clip, "randomizeVolume", false);
            SetPrivateField(_clip, "randomizePitch", false);
            SetPrivateField(_clip, "volume", new RangedFloat { MinValue = 1f, MaxValue = 1f });
            SetPrivateField(_clip, "pitch", new RangedFloat { MinValue = 0.75f, MaxValue = 1.5f });

            _audioSource.pitch = 1.2f;
            _clip.Play(_audioSource);

            Assert.That(_audioSource.pitch, Is.EqualTo(1.2f).Within(0.001f),
                "Pitch should remain unchanged when randomization is disabled");

            AudioClip.DestroyImmediate(audioClip);
        }

        [Test]
        public void Play_SetsVolumeWithinRange_WhenRandomized()
        {
            var audioClip = AudioClip.Create("TestClip", 44100, 1, 44100, false);
            SetPrivateField(_clip, "audioClip", audioClip);
            SetPrivateField(_clip, "randomizeVolume", true);
            SetPrivateField(_clip, "randomizePitch", false);
            SetPrivateField(_clip, "volume", new RangedFloat { MinValue = 0.3f, MaxValue = 0.7f });
            SetPrivateField(_clip, "pitch", new RangedFloat { MinValue = 1f, MaxValue = 1f });

            _clip.Play(_audioSource);

            Assert.That(_audioSource.volume,
                Is.GreaterThanOrEqualTo(0.3f).And.LessThanOrEqualTo(0.7f));

            AudioClip.DestroyImmediate(audioClip);
        }

        [Test]
        public void AudioClipProperty_ReturnsAssignedClip()
        {
            var audioClip = AudioClip.Create("TestClip", 44100, 1, 44100, false);
            SetPrivateField(_clip, "audioClip", audioClip);

            Assert.That(_clip.AudioClip, Is.EqualTo(audioClip));

            AudioClip.DestroyImmediate(audioClip);
        }

        private static void SetPrivateField(object target, string fieldName, object value)
        {
            var type = target.GetType();
            var field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.That(field, Is.Not.Null, $"Field '{fieldName}' not found on {type.Name}");
            field.SetValue(target, value);
        }
    }
}
