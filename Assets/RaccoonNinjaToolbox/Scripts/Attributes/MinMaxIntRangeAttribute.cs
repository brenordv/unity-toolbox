using System;

namespace RaccoonNinjaToolbox.Scripts.Attributes
{
    public class MinMaxIntRangeAttribute : Attribute
    {
        public MinMaxIntRangeAttribute(int min = 0, int max = 1)
        {
            Min = min;
            Max = max;
        }

        public int Min { get; }
        public int Max { get; }
    }
}