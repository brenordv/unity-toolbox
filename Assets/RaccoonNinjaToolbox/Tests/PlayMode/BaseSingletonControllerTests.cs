using System.Collections;
using NUnit.Framework;
using RaccoonNinjaToolbox.Tests.PlayMode.Helpers;
using UnityEngine;
using UnityEngine.TestTools;

namespace RaccoonNinjaToolbox.Tests.PlayMode
{
    [TestFixture]
    public class BaseSingletonControllerTests
    {
        private GameObject _primaryGo;

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            if (_primaryGo) Object.Destroy(_primaryGo);
            yield return null;
        }

        [UnityTest]
        public IEnumerator Awake_FirstInstance_BecomesInstance()
        {
            _primaryGo = new GameObject("Singleton");
            var component = _primaryGo.AddComponent<TestSingleton>();
            yield return null;

            Assert.That(TestSingleton.Instance, Is.EqualTo(component));
        }

        [UnityTest]
        public IEnumerator Awake_SecondInstance_GetsDestroyed()
        {
            _primaryGo = new GameObject("Singleton_First");
            var first = _primaryGo.AddComponent<TestSingleton>();
            yield return null;

            var secondGo = new GameObject("Singleton_Second");
            secondGo.AddComponent<TestSingleton>();
            yield return null;

            Assert.That(TestSingleton.Instance, Is.EqualTo(first),
                "First instance should remain as the singleton");
            Assert.That(!secondGo,
                "Second GameObject should have been destroyed");
        }

        [UnityTest]
        public IEnumerator OnDestroy_ClearsInstance()
        {
            _primaryGo = new GameObject("Singleton");
            _primaryGo.AddComponent<TestSingleton>();
            yield return null;

            Assert.That(TestSingleton.Instance, Is.Not.Null, "Instance should exist before destroy");

            Object.Destroy(_primaryGo);
            yield return null;
            _primaryGo = null;

            Assert.That(!TestSingleton.Instance, "Instance should be null after destroy");
        }

        [UnityTest]
        public IEnumerator PostAwake_IsCalledOnFirstInstance()
        {
            _primaryGo = new GameObject("Singleton");
            var component = _primaryGo.AddComponent<TestSingleton>();
            yield return null;

            Assert.That(component.PostAwakeCalled, Is.True);
            Assert.That(component.PostAwakeCallCount, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator PostAwake_IsNotCalledOnDuplicateInstance()
        {
            _primaryGo = new GameObject("Singleton_First");
            var first = _primaryGo.AddComponent<TestSingleton>();
            yield return null;

            var secondGo = new GameObject("Singleton_Second");
            var second = secondGo.AddComponent<TestSingleton>();
            yield return null;

            Assert.That(first.PostAwakeCallCount, Is.EqualTo(1),
                "First instance should have PostAwake called exactly once");
        }
    }
}
