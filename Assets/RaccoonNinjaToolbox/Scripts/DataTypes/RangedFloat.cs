using System;

namespace RaccoonNinjaToolbox.Scripts.DataTypes
{
    /// <summary>
    /// By default, min value is zero and max is 1. If needed, pair this property with MinMaxFloatRangeAttribute to set
    /// other bounds.
    /// <remarks>The fields must stay without getters and setters because the way SerializedProperty works. (and I
    /// didn't want to add unnecessary complexity here)</remarks>
    /// </summary>
    [Serializable]
    public struct RangedFloat
    {
        public float MinValue;
        public float MaxValue;

        public float Random()
        {
            return UnityEngine.Random.Range(MinValue, MaxValue);
        }
    }
}