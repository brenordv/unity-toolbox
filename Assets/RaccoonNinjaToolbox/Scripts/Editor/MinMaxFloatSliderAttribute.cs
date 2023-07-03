using RaccoonNinjaToolbox.Scripts.Attributes;
using RaccoonNinjaToolbox.Scripts.DataTypes;
using UnityEditor;
using UnityEngine;

namespace RaccoonNinjaToolbox.Scripts.Editor
{
    // Custom Property Drawer that enables us to create a user interface slider with min and max values.
    [CustomPropertyDrawer(typeof(RangedFloat), true)]
    public class MinMaxFloatSliderAttribute : PropertyDrawer
    {
        // A constant field width for the input fields that handle the minimum and maximum range values.
        private const float RangeBoundsFieldWidth = 60f;

        // OnGUI is called by Unity to draw the control in the inspector.
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Begin drawing the property using the given layout settings.
            label = EditorGUI.BeginProperty(position, label, property);

            // Draw a label in front of the property.
            position = EditorGUI.PrefixLabel(position, label);

            // Find and get the MinValue and MaxValue properties relative to the SerializedProperty.
            var minProp = property.FindPropertyRelative("MinValue");
            var maxProp = property.FindPropertyRelative("MaxValue");

            // Get the current minimum and maximum values.
            var minValue = minProp.floatValue;
            var maxValue = maxProp.floatValue;

            // Set a default minimum and maximum allowed values for the slider.
            var rangeMin = 0f;
            var rangeMax = 1f;

            // Get the MinMaxFloatRangeAttribute from the field, if any.
            var ranges =
                (MinMaxFloatRangeAttribute[])fieldInfo.GetCustomAttributes(typeof(MinMaxFloatRangeAttribute), true);

            // If there are any MinMaxFloatRangeAttributes, use their min and max values.
            if (ranges.Length > 0)
            {
                rangeMin = ranges[0].Min;
                rangeMax = ranges[0].Max;
            }

            // Create a rect for the minimum range input field and draw the field.
            var minFieldRect = new Rect(position)
            {
                width = RangeBoundsFieldWidth
            };
            minValue = DrawRangeField(minFieldRect, minValue);

            // Shift the position to the right by the width of the min field.
            position.xMin += RangeBoundsFieldWidth;

            // Create a rect for the maximum range input field and draw the field.
            var maxFieldRect = new Rect(position)
            {
                xMin = position.xMax - RangeBoundsFieldWidth
            };
            maxValue = DrawRangeField(maxFieldRect, maxValue);

            // Shift the position to the left by the width of the max field.
            position.xMax -= RangeBoundsFieldWidth;

            // Start checking for changes in the GUI.
            EditorGUI.BeginChangeCheck();

            // Draw the slider.
            EditorGUI.MinMaxSlider(position, ref minValue, ref maxValue, rangeMin, rangeMax);

            // If changes were made in the GUI, assign the new values to the serialized properties.
            if (EditorGUI.EndChangeCheck())
            {
                minProp.floatValue = minValue;
                maxProp.floatValue = maxValue;
            }

            // End drawing the property.
            EditorGUI.EndProperty();
        }

        // Draws an input field for a range and returns the entered value as a float.
        private static float DrawRangeField(Rect fieldRect, float value)
        {
            // Draw the TextField and get the value as a string.
            var valueString = EditorGUI.TextField(fieldRect, value.ToString("F2"));

            // Parse the string as a float and return it.
            return float.Parse(valueString);
        }
    }
}