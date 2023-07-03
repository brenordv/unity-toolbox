using System;
using RaccoonNinjaToolbox.Scripts.Abstractions.DataTypes;

namespace RaccoonNinjaToolbox.Scripts.DataTypes
{
    [Serializable]
    public class RangedFloat: RangedNumeric<float>
    {
        /// <summary>
        /// Gets a random value between MinValue and MaxValue (inclusive).
        /// </summary>
        /// <returns>Random int in range</returns>
        public override float Random()
        {
            return UnityEngine.Random.Range(MinValue, MaxValue);
        }
    }
}