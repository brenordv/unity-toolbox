using System;
using System.Collections.Generic;
using UnityEngine;

namespace RaccoonNinjaToolbox.Scripts.Abstractions.Controllers
{
    internal static class SingletonResetter
    {
        private static readonly List<Action> ResetActions = new();

        internal static void Register(Action resetAction)
        {
            if (!ResetActions.Contains(resetAction))
                ResetActions.Add(resetAction);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetAll()
        {
            foreach (var action in ResetActions)
                action();
        }
    }
}
