using System;

namespace RaccoonNinjaToolbox.Scripts.Abstractions.DataTypes
{
    /// <summary>
    /// By default, min value is zero and max is 1. If needed, pair this property with MinMaxFloatRangeAttribute to set
    /// other bounds.
    /// <remarks>The fields must stay without getters and setters because the way SerializedProperty works. (and I
    /// didn't want to add unnecessary complexity here)</remarks>
    /// </summary>
    [Serializable]
    public abstract class RangedNumeric<T> where T : struct
    {
        public T MinValue;
        public T MaxValue;

        public abstract T Random();
    }
}