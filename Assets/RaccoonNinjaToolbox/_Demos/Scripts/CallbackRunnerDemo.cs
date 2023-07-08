using System;
using System.Collections;
using System.Collections.Generic;
using RaccoonNinjaToolbox.Scripts.GlobalControllers;
using UnityEngine;

namespace RaccoonNinjaToolbox._Demo.Scripts
{
    public class CallbackRunnerDemo : MonoBehaviour
    {
        private Guid _latestCoroutineKey;

        public void RoutineStarted(Guid key)
        {
            Debug.Log($"[Event Received: Started] Coroutine with key {key} started.");
        }
        
        public void RoutineFinished(Guid key)
        {
            Debug.Log($"[Event Received: Finished]Coroutine with key {key} finished.");
        }
        
        public void RoutineStopped(Guid key)
        {
            Debug.Log($"[Event Received: Stopped] Coroutine with key {key} stopped.");
        }
        
        public void RunFiveSecondsRoutine()
        {
            _latestCoroutineKey = CallbackRunner.Instance.StartCoroutineImmediately(CountFiveSeconds);
        }

        public void RunFiveSecondsRoutineAfter2Secs()
        {
            _latestCoroutineKey = CallbackRunner.Instance.StartCoroutineAfterDelay(2, CountFiveSeconds);
        }

        public void RunNormalFunctionAsCallback()
        {
            _latestCoroutineKey = CallbackRunner.Instance.StartCoroutineImmediately(GetFirst100FibonacciNumbers);
        }

        public void RunNormalFunctionAsCallbackAfter2Secs()
        {
            _latestCoroutineKey = CallbackRunner.Instance.StartCoroutineAfterDelay(2, GetFirst100FibonacciNumbers);
        }
        
        public void CancelLatestCoroutine()
        {
            Debug.Log(CallbackRunner.Instance.StopCoroutine(_latestCoroutineKey)
                ? $"Coroutine with key {_latestCoroutineKey} stopped."
                : $"Coroutine with key {_latestCoroutineKey} not found.");
        }

        private IEnumerator CountFiveSeconds()
        {
            var seconds = 0;

            while (seconds < 5)
            {
                Debug.Log($"{seconds} seconds passed.");
                yield return new WaitForSeconds(1);
                seconds++;
            }
        }

        private void GetFirst100FibonacciNumbers()
        {
            const int count = 100;
            int n1 = 0, n2 = 1, n3;

            var fibonacciSequence = new List<int> { n1, n2 }; // List to hold the Fibonacci sequence

            for (var i = 2; i < count; ++i)
            {
                n3 = n1 + n2;
                fibonacciSequence.Add(n3);
                n1 = n2;
                n2 = n3;
            }

            // Join all numbers in the list with space separator and print them
            Debug.Log($"First {count} Fibonacci numbers:");
            Debug.Log(string.Join(" ", fibonacciSequence));
        }
    }
}
