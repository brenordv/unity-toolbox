using System;
using System.Collections;
using System.Reflection;
using NUnit.Framework;
using RaccoonNinjaToolbox.Scripts.GlobalControllers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TestTools;

namespace RaccoonNinjaToolbox.Tests.PlayMode
{
    [TestFixture]
    public class CallbackRunnerTests
    {
        private GameObject _runnerGo;
        private CallbackRunner _runner;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            _runnerGo = new GameObject("CallbackRunner");
            _runner = _runnerGo.AddComponent<CallbackRunner>();
            yield return null;
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            if (_runnerGo) UnityEngine.Object.Destroy(_runnerGo);
            yield return null;
        }

        #region StartCoroutineImmediately

        [UnityTest]
        public IEnumerator StartCoroutineImmediately_WithAction_ReturnsNonEmptyGuid()
        {
            var key = _runner.StartCoroutineImmediately(() => { });
            yield return null;

            Assert.That(key, Is.Not.EqualTo(Guid.Empty));
        }

        [UnityTest]
        public IEnumerator StartCoroutineImmediately_WithAction_ExecutesAction()
        {
            var executed = false;

            _runner.StartCoroutineImmediately(() => executed = true);
            yield return null;
            yield return null;

            Assert.That(executed, Is.True, "Action should have been executed");
        }

        [UnityTest]
        public IEnumerator StartCoroutineImmediately_WithFunc_ReturnsNonEmptyGuid()
        {
            IEnumerator TestRoutine()
            {
                yield return null;
            }

            var key = _runner.StartCoroutineImmediately(TestRoutine);
            yield return null;

            Assert.That(key, Is.Not.EqualTo(Guid.Empty));
        }

        [UnityTest]
        public IEnumerator StartCoroutineImmediately_WithFunc_ExecutesRoutine()
        {
            var executed = false;

            IEnumerator TestRoutine()
            {
                executed = true;
                yield return null;
            }

            _runner.StartCoroutineImmediately(TestRoutine);
            yield return null;
            yield return null;

            Assert.That(executed, Is.True, "Coroutine body should have been executed");
        }

        #endregion

        #region StartCoroutineAfterDelay

        [UnityTest]
        public IEnumerator StartCoroutineAfterDelay_WithAction_ReturnsNonEmptyGuid()
        {
            var key = _runner.StartCoroutineAfterDelay(0.01f, () => { });
            yield return null;

            Assert.That(key, Is.Not.EqualTo(Guid.Empty));
        }

        [UnityTest]
        public IEnumerator StartCoroutineAfterDelay_WithFunc_ReturnsNonEmptyGuid()
        {
            IEnumerator TestRoutine()
            {
                yield return null;
            }

            var key = _runner.StartCoroutineAfterDelay(0.01f, TestRoutine);
            yield return null;

            Assert.That(key, Is.Not.EqualTo(Guid.Empty));
        }

        #endregion

        #region StopCoroutine

        [UnityTest]
        public IEnumerator StopCoroutine_WithEmptyGuid_ReturnsFalse()
        {
            yield return null;

            Assert.That(_runner.StopCoroutine(Guid.Empty), Is.False);
        }

        [UnityTest]
        public IEnumerator StopCoroutine_WithNonExistentKey_ReturnsFalse()
        {
            yield return null;

            Assert.That(_runner.StopCoroutine(Guid.NewGuid()), Is.False);
        }

        [UnityTest]
        public IEnumerator StopCoroutine_WithValidKey_ReturnsTrue()
        {
            IEnumerator LongRunningRoutine()
            {
                yield return new WaitForSeconds(10f);
            }

            var key = _runner.StartCoroutineImmediately(LongRunningRoutine);
            yield return null;
            yield return null;

            Assert.That(_runner.StopCoroutine(key), Is.True);
        }

        [UnityTest]
        public IEnumerator StopCoroutine_PreventsRoutineFromCompleting()
        {
            var completed = false;

            IEnumerator LongRunningRoutine()
            {
                yield return new WaitForSeconds(10f);
                completed = true;
            }

            var key = _runner.StartCoroutineImmediately(LongRunningRoutine);
            yield return null;
            yield return null;

            _runner.StopCoroutine(key);
            yield return null;

            Assert.That(completed, Is.False, "Routine should not have completed after being stopped");
        }

        #endregion

        #region Events

        [UnityTest]
        public IEnumerator OnCoroutineStarted_EventFires()
        {
            Guid firedKey = Guid.Empty;
            var eventField = GetPrivateEvent("onCoroutineStarted");
            eventField.AddListener(key => firedKey = key);

            var routineKey = _runner.StartCoroutineImmediately(() => { });
            yield return null;
            yield return null;

            Assert.That(firedKey, Is.Not.EqualTo(Guid.Empty), "onCoroutineStarted should have fired");
            Assert.That(firedKey, Is.EqualTo(routineKey));
        }

        [UnityTest]
        public IEnumerator OnCoroutineFinished_EventFires()
        {
            Guid firedKey = Guid.Empty;
            var eventField = GetPrivateEvent("onCoroutineFinished");
            eventField.AddListener(key => firedKey = key);

            var routineKey = _runner.StartCoroutineImmediately(() => { });

            // Wait enough frames for the coroutine to start, run, and finish
            yield return null;
            yield return null;
            yield return null;

            Assert.That(firedKey, Is.Not.EqualTo(Guid.Empty), "onCoroutineFinished should have fired");
            Assert.That(firedKey, Is.EqualTo(routineKey));
        }

        [UnityTest]
        public IEnumerator OnCoroutineStopped_EventFires()
        {
            Guid firedKey = Guid.Empty;
            var eventField = GetPrivateEvent("onCoroutineStopped");
            eventField.AddListener(key => firedKey = key);

            IEnumerator LongRunningRoutine()
            {
                yield return new WaitForSeconds(10f);
            }

            var routineKey = _runner.StartCoroutineImmediately(LongRunningRoutine);
            yield return null;
            yield return null;

            _runner.StopCoroutine(routineKey);

            Assert.That(firedKey, Is.Not.EqualTo(Guid.Empty), "onCoroutineStopped should have fired");
            Assert.That(firedKey, Is.EqualTo(routineKey));
        }

        #endregion

        #region Helpers

        private UnityEvent<Guid> GetPrivateEvent(string fieldName)
        {
            var field = typeof(CallbackRunner)
                .GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.That(field, Is.Not.Null, $"Field '{fieldName}' not found on CallbackRunner");

            var evt = field.GetValue(_runner) as UnityEvent<Guid>;
            if (evt == null)
            {
                evt = new UnityEvent<Guid>();
                field.SetValue(_runner, evt);
            }

            return evt;
        }

        #endregion
    }
}
