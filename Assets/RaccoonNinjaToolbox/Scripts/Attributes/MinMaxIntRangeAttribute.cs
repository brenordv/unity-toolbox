using System;
using RaccoonNinjaToolbox.Scripts.Interfaces;

namespace RaccoonNinjaToolbox.Scripts.Attributes
{
    public class MinMaxIntRangeAttribute : Attribute, IMinMaxRangeAttribute<int>
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