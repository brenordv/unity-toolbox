using System;
using RaccoonNinjaToolbox.Scripts.Interfaces;

namespace RaccoonNinjaToolbox.Scripts.Attributes
{
    public class MinMaxFloatRangeAttribute : Attribute, IMinMaxRangeAttribute<float>
    {
        public MinMaxFloatRangeAttribute(float min = 0f, float max = 1f)
        {
            Min = min;
            Max = max;
        }

        public float Min { get; }
        public float Max { get; }
    }
}