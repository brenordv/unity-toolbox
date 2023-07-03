using System;
using System.Collections.Generic;
using RaccoonNinjaToolbox.Scripts.Abstractions.DataTypes;
using UnityEditor;
using UnityEngine;

namespace RaccoonNinjaToolbox.Scripts.Editor
{
    // Custom Property Drawer that enables us to create a user interface slider with min and max values.
    public abstract class NumericSliderDrawer<TControllerAttribute, TEntity> : PropertyDrawer
        where TEntity : struct
        where TControllerAttribute : Attribute
    {
        // A constant field width for the input fields that handle the minimum and maximum range values.
        private const float RangeBoundsFieldWidth = 60f;
        
                // OnGUI is called by Unity to draw the control in the inspector.
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            try
            {
                if (property == null) return;

                // Begin drawing the property using the given layout settings.
                label = EditorGUI.BeginProperty(position, label, property);

                // Draw a label in front of the property.
                position = EditorGUI.PrefixLabel(position, label);



                // Find and get the MinValue and MaxValue properties relative to the SerializedProperty.
                var minProp = property.FindPropertyRelative("MinValue");
                var maxProp = property.FindPropertyRelative("MaxValue");

                if (minProp == null || maxProp == null) return;

                // Get the current minimum and maximum values.
                var minValue = GetValueFromProperty(minProp);
                var maxValue = GetValueFromProperty(maxProp);

                // Set a default minimum and maximum allowed values for the slider.
                var rangeMin = GetDefaultRangeMin(minValue);
                var rangeMax = GetDefaultRangeMax(maxValue);

                // Get the MinMaxFloatRangeAttribute from the field, if any.
                var ranges =
                    (TControllerAttribute[])fieldInfo.GetCustomAttributes(typeof(TControllerAttribute), true);

                // If there are any MinMaxFloatRangeAttributes, use their min and max values.
                if (ranges.Length > 0)
                {
                    rangeMin = GetValueFromControllerAttribute(ranges[0], "Min");
                    rangeMax = GetValueFromControllerAttribute(ranges[0], "Max");
                }

                // Create a rect for the minimum range input field and draw the field.
                var minFieldRect = new Rect(position)
                {
                    width = RangeBoundsFieldWidth
                };
                var result = DrawRangeField(minFieldRect, minProp);
                
                if (!result.success)
                    return;

                minValue = result.value;
                
                // Shift the position to the right by the width of the min field.
                position.xMin += RangeBoundsFieldWidth;

                // Create a rect for the maximum range input field and draw the field.
                var maxFieldRect = new Rect(position)
                {
                    xMin = position.xMax - RangeBoundsFieldWidth
                };

                result = DrawRangeField(maxFieldRect, maxProp);

                if (!result.success)
                    return;

                maxValue = result.value;

                // Make sure max value is not less than min value.
                if (GetEntityValueAsFloat(maxValue) < GetEntityValueAsFloat(minValue))
                {
                    maxValue = minValue;
                    SetValueToProperty(maxProp, GetEntityValueAsFloat(maxValue));
                }
                
                // Shift the position to the left by the width of the max field.
                position.xMax -= RangeBoundsFieldWidth;

                // Start checking for changes in the GUI.
                EditorGUI.BeginChangeCheck();

                var sliderMinValue = GetEntityValueAsFloat(minValue);
                var sliderMaxValue = GetEntityValueAsFloat(maxValue);

                // If the min and max values are the same...
                if (Mathf.Abs(sliderMaxValue - sliderMinValue) < Mathf.Epsilon)
                {
                    // ...add a small offset to the max value to prevent the slider from disappearing.
                    // Note: This will not actually change the max value of the property.
                    sliderMaxValue = sliderMinValue + 0.005f;
                }
                
                // Draw the slider.
                EditorGUI.MinMaxSlider(position, ref sliderMinValue, ref sliderMaxValue, rangeMin, rangeMax);

                // If changes were made in the GUI, assign the new values to the serialized properties.
                if (EditorGUI.EndChangeCheck())
                {
                    SetValueToProperty(minProp, sliderMinValue);
                    SetValueToProperty(maxProp, sliderMaxValue);
                }

                // End drawing the property.
                EditorGUI.EndProperty();
            }
            catch (FormatException fe)
            {
                Debug.LogWarning($"{property.name} - {fe.Message}");
            }
            catch (ArgumentNullException ane)
            {
                Debug.LogWarning($"{property.name} - {ane.Message}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to render numeric slider. Error: {e.Message}");
                throw;
            }

        }

        /// <summary>
        /// Draws a text field, reads the entered value, and immediately updates the corresponding property.
        /// </summary>
        /// <param name="fieldRect">Rectangular area to draw the text field in.</param>
        /// <param name="property">Serialized property to update.</param>
        /// <returns>The entered value parsed to TEntity.</returns>
        private (TEntity value, bool success) DrawRangeField(Rect fieldRect, SerializedProperty property)
        {
            // Get the current value of the property.
            var value = GetValueFromProperty(property);
            
            // Draw the text field and get the entered value as a string.
            var valueString = EditorGUI.TextField(fieldRect, ToFormattedString(value));

            // Parse the entered string to TEntity.
            var (parsedValue, success) = Parse(valueString);

            if (!success)
                return (default, false);

            // If the entered value is different from the current value of the property...
            if (!EqualityComparer<TEntity>.Default.Equals(parsedValue, value))
            {
                // ...update the property with the entered value.
                SetValueToProperty(property, GetEntityValueAsFloat(parsedValue));
            }

            // Return the entered value.
            return (parsedValue, true);
        }


        private static float GetValueFromControllerAttribute(TControllerAttribute attr, string propName)
        {
            // Get the type of the instance
            var type = attr.GetType();

            // Get the PropertyInfo for the Min property
            var prop = type.GetProperty(propName);

            // If the property doesn't exist, return null
            if (prop == null)
                return default;
            
            // Use the PropertyInfo to get the value of the Min property
            var value = prop.GetValue(attr);

            return value is int intValue ? Convert.ToSingle(intValue) : default;
        }
        
        protected abstract float GetDefaultRangeMin(TEntity value);
        protected abstract float GetDefaultRangeMax(TEntity value);
        protected abstract float GetEntityValueAsFloat(TEntity value);
        protected abstract TEntity GetValueFromProperty(SerializedProperty property);
        protected abstract void SetValueToProperty(SerializedProperty property, float value);
        protected abstract (TEntity value, bool success) Parse(string valueString);
        protected abstract string ToFormattedString(TEntity value);
    }
}