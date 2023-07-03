using System;
using RaccoonNinjaToolbox.Scripts.Abstractions.DataTypes;

namespace RaccoonNinjaToolbox.Scripts.DataTypes
{
    [Serializable]
    public class RangedInt: RangedNumeric<int>
    {
        /// <summary>
        /// Gets a random value between MinValue and MaxValue (inclusive).
        /// </summary>
        /// <returns>Random int in range</returns>
        public override int Random()
        {
            return UnityEngine.Random.Range(MinValue, MaxValue +1);
        }
    }
}