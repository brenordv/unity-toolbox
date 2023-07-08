using System;
using System.Collections;
using System.Collections.Generic;
using RaccoonNinjaToolbox.Scripts.Abstractions.Controllers;
using RaccoonNinjaToolbox.Scripts.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace RaccoonNinjaToolbox.Scripts.GlobalControllers
{
    public class CallbackRunner : BaseSingletonController<CallbackRunner>
    {
        /// <summary>
        /// Called whenever a coroutine is started.
        /// The Guid is the Key to that coroutine.
        /// </summary>
        [SerializeField] private UnityEvent<Guid> onCoroutineStarted;
        
        /// <summary>
        /// Called whenever a coroutine is finished.
        /// The Guid is the Key to that coroutine.
        /// </summary>
        [SerializeField] private UnityEvent<Guid> onCoroutineFinished;
        
        /// <summary>
        /// Called whenever a coroutine is stopped by the code.
        /// The Guid is the Key to that coroutine.
        /// </summary>
        [SerializeField] private UnityEvent<Guid> onCoroutineStopped;

        [Space(10), Header("Debug Settings")]
        [SerializeField] private bool enableDebugLog;
        [SerializeField] private bool enableRoutineKeyRuntimeInfo;
        [SerializeField, InspectorReadOnly] private List<string> runningCoroutineKeys;

        private IDictionary<Guid, Coroutine> _routines;
        
        protected override void PostAwake()
        {
            _routines = new Dictionary<Guid, Coroutine>();
        }

        /// <summary>
        /// Starts a Coroutine immediately and returns a key that can be used to cancel the routine later.
        /// </summary>
        /// <param name="coroutine">Routine that will be executed in the Coroutine</param>
        /// <returns>Guid representing the key to that coroutine.</returns>
        public Guid StartCoroutineImmediately(Func<IEnumerator> coroutine)
        {
            var newKey = GetCoroutineKey();
            
            StartCoroutine(StartManagedCoroutine(coroutine, newKey));

            return newKey;
        }

        /// <summary>
        /// Starts a Coroutine immediately and returns a key that can be used to cancel the routine later.
        /// </summary>
        /// <param name="action">Action that will be executed in the Coroutine</param>
        /// <returns>Guid representing the key to that coroutine.</returns>
        public Guid StartCoroutineImmediately(Action action)
        {
            var newKey = GetCoroutineKey();
            
            StartCoroutine(StartManagedCoroutine(WrapActionInEnumerator(action), newKey));

            return newKey;
        }
        
        /// <summary>
        /// Will run after a delay. The key to that coroutine will be returned immediately.
        /// </summary>
        /// <param name="delay">Time to wait before running action.</param>
        /// <param name="action">Delegate that will be executed.</param>
        /// <returns>Key to the coroutine</returns>
        public Guid StartCoroutineAfterDelay(float delay, Action action)
        {
            var newKey = GetCoroutineKey();

            StartCoroutine(StartManagedCoroutine(WrapActionInEnumerator(action), newKey, delay));

            return newKey;
        }

        /// <summary>
        /// Will run after a delay. The key to that coroutine will be returned immediately. 
        /// </summary>
        /// <param name="delay">Time to wait before running action.</param>
        /// <param name="routine">IEnumerator that will be executed.</param>
        /// <returns>Key to the coroutine</returns>
        public Guid StartCoroutineAfterDelay(float delay, Func<IEnumerator> routine)
        {
            var newKey = GetCoroutineKey();

            StartCoroutine(StartManagedCoroutine(routine, newKey, delay));

            return newKey;
        }
        
        /// <summary>
        /// Tries to stop a Coroutine according to the key provided.
        /// </summary>
        /// <param name="coroutineKey">Coroutine key</param>
        /// <returns>true if the key exists and the routine was stopped, false otherwise.</returns>
        public bool StopCoroutine(Guid coroutineKey)
        {
            if (coroutineKey == Guid.Empty || !_routines.ContainsKey(coroutineKey)) return false;

            StopCoroutine(_routines[coroutineKey]);

            Log($"Coroutine '{coroutineKey}' stopped");

            onCoroutineStopped?.Invoke(coroutineKey);

            RemoveCoroutine(coroutineKey);

            return true;
        }

        private IEnumerator StartManagedCoroutine(Func<IEnumerator> action, Guid coroutineKey, float? delay = null)
        {
            if (delay.HasValue)
            {
                Log($"Delaying routine start by {delay.Value} seconds");

                var delayCoroutine = StartCoroutine(WaitForDelay(delay.Value));

                RegisterNewCoroutine(coroutineKey, delayCoroutine);

                yield return delayCoroutine;

                if (IsCoroutineCancelled(coroutineKey)) yield break;
            }

            Log($"Coroutine '{coroutineKey}' started");
            
            onCoroutineStarted.Invoke(coroutineKey);

            var coroutine = StartCoroutine(action());

            RegisterNewCoroutine(coroutineKey, coroutine);

            yield return coroutine;
            
            Log($"Coroutine '{coroutineKey}' finished");

            onCoroutineFinished?.Invoke(coroutineKey);

            RemoveCoroutine(coroutineKey);
        }

        private void RegisterNewCoroutine(Guid coroutineKey, Coroutine runningCoroutine)
        {
            _routines.Add(coroutineKey, runningCoroutine);
            if (!enableRoutineKeyRuntimeInfo) return;
            runningCoroutineKeys.Add(coroutineKey.ToString());
        }
        
        private void RemoveCoroutine(Guid coroutineKey)
        {
            _routines.Remove(coroutineKey);
            if (!enableRoutineKeyRuntimeInfo) return;
            runningCoroutineKeys.Remove(coroutineKey.ToString());
        }

        private bool IsCoroutineCancelled(Guid key) => !_routines.ContainsKey(key);
        
        private static Guid GetCoroutineKey() => Guid.NewGuid();

        private static IEnumerator WaitForDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
        }
        
        private static Func<IEnumerator> WrapActionInEnumerator(Action action)
        {
            IEnumerator Wrapper()
            {
                action();
                yield return null;
            }

            return Wrapper;
        } 
        
        private void Log(string message)
        {
            if (!enableDebugLog) return;

            Debug.Log(message);
        }
    }
}