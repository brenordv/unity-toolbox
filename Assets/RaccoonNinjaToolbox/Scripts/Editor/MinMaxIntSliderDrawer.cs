using System;
using RaccoonNinjaToolbox.Scripts.Abstractions.Editor;
using RaccoonNinjaToolbox.Scripts.Attributes;
using RaccoonNinjaToolbox.Scripts.DataTypes;
using UnityEditor;
using UnityEngine;

namespace RaccoonNinjaToolbox.Scripts.Editor
{
    [CustomPropertyDrawer(typeof(RangedInt), true)]
    public class MinMaxIntSliderDrawer: NumericSliderDrawer<MinMaxIntRangeAttribute, int>
    {
        protected override float ObjectToFloat(object value)
        {
            return value is int intValue ? Convert.ToSingle(intValue) : default;
        }

        protected override float GetDefaultRangeMin()
        {
            return 0;
        }

        protected override float GetDefaultRangeMax()
        {
            return 1;
        }

        protected override float GetEntityValueAsFloat(int value)
        {
            return value;
        }

        protected override int GetValueFromProperty(SerializedProperty property)
        {
            return property.intValue;
        }

        protected override void SetValueToProperty(SerializedProperty property, float value)
        {
            property.intValue = Mathf.RoundToInt(value);
        }

        protected override (int value, bool success) Parse(string valueString)
        {
            return int.TryParse(valueString, out var value) ? (value, true) : (default, false);
        }

        protected override string ToFormattedString(int value)
        {
            return value.ToString();
        }
    }
}