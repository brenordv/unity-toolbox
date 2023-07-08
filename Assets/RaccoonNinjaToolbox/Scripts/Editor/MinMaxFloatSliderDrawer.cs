using RaccoonNinjaToolbox.Scripts.Abstractions.Editor;
using RaccoonNinjaToolbox.Scripts.Attributes;
using RaccoonNinjaToolbox.Scripts.DataTypes;
using UnityEditor;
using UnityEngine;

namespace RaccoonNinjaToolbox.Scripts.Editor
{
    [CustomPropertyDrawer(typeof(RangedFloat), true)]
    public class MinMaxFloatSliderDrawer: NumericSliderDrawer<MinMaxFloatRangeAttribute, float>
    {
        protected override float ObjectToFloat(object value)
        {
            return value is float floatValue ? floatValue : default;
        }

        protected override float GetDefaultRangeMin()
        {
            return 0;
        }

        protected override float GetDefaultRangeMax()
        {
            return 1;
        }

        protected override float GetEntityValueAsFloat(float value)
        {
            return value;
        }

        protected override float GetValueFromProperty(SerializedProperty property)
        {
            return property.floatValue;
        }

        protected override void SetValueToProperty(SerializedProperty property, float value)
        {
            property.floatValue = value;
        }

        protected override (float value, bool success) Parse(string valueString)
        {
            return float.TryParse(valueString, out var value) ? (value, true) : (default, false);
        }

        protected override string ToFormattedString(float value)
        {
            return value.ToString("F2");
        }
    }
}