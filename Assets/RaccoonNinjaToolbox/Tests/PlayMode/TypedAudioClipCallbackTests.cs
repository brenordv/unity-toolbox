using System.Collections;
using System.Reflection;
using System.Threading;
using NUnit.Framework;
using RaccoonNinjaToolbox.Scripts.DataTypes;
using RaccoonNinjaToolbox.Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.TestTools;

namespace RaccoonNinjaToolbox.Tests.PlayMode
{
    [TestFixture]
    public class TypedAudioClipCallbackTests
    {
        private const float TestDuration = 0.15f;

        private TypedAudioClip _clip;
        private GameObject _audioGo;
        private AudioSource _audioSource;
        private AudioClip _audioClip;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            _clip = ScriptableObject.CreateInstance<TypedAudioClip>();
            _audioGo = new GameObject("TestAudioSource");
            _audioSource = _audioGo.AddComponent<AudioSource>();
            _audioClip = AudioClip.Create("TestClip", 44100, 1, 44100, false);

            SetPrivateField(_clip, "audioClip", _audioClip);
            SetPrivateField(_clip, "randomizeVolume", false);
            SetPrivateField(_clip, "randomizePitch", false);
            SetBackingField(_clip, "PracticalDuration", TestDuration);

            yield return null;
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            if (_clip) Object.Destroy(_clip);
            if (_audioGo) Object.Destroy(_audioGo);
            if (_audioClip) Object.Destroy(_audioClip);
            yield return null;
        }

        [UnityTest]
        public IEnumerator Play_WithCallback_InvokesCallbackAfterPracticalDuration()
        {
            var callbackInvoked = false;

            _clip.Play(_audioSource, () => callbackInvoked = true);

            Assert.That(callbackInvoked, Is.False, "Callback should not fire immediately");

            yield return new WaitForSeconds(TestDuration + 0.1f);

            Assert.That(callbackInvoked, Is.True, "Callback should have fired after PracticalDuration");
        }

        [UnityTest]
        public IEnumerator Play_WithCanceledToken_DoesNotInvokeCallback()
        {
            var callbackInvoked = false;
            var cts = new CancellationTokenSource();

            _clip.Play(_audioSource, () => callbackInvoked = true, cts.Token);

            yield return null;

            cts.Cancel();
            cts.Dispose();

            yield return new WaitForSeconds(TestDuration + 0.1f);

            Assert.That(callbackInvoked, Is.False, "Callback should not fire when token is canceled");
        }

        [UnityTest]
        public IEnumerator Play_WithoutCallback_DoesNotThrow()
        {
            _clip.Play(_audioSource);

            yield return new WaitForSeconds(TestDuration + 0.1f);

            Assert.Pass("No exception thrown when playing without a callback");
        }

        private static void SetPrivateField(object target, string fieldName, object value)
        {
            var type = target.GetType();
            var field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.That(field, Is.Not.Null, $"Field '{fieldName}' not found on {type.Name}");
            field.SetValue(target, value);
        }

        private static void SetBackingField(object target, string propertyName, object value)
        {
            var type = target.GetType();
            var field = type.GetField($"<{propertyName}>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.That(field, Is.Not.Null, $"Backing field for '{propertyName}' not found on {type.Name}");
            field.SetValue(target, value);
        }
    }
}
