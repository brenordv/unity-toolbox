using RaccoonNinjaToolbox.Scripts.Attributes;
using UnityEditor;
using UnityEngine;

namespace RaccoonNinjaToolbox.Scripts.Editor
{
    /// <summary>
    /// Most simple way I could find to do this.
    /// Nothing fancy, but it works fine.
    /// </summary>
    /// <remarks>
    /// From: https://github.com/brenordv/unity-tag-selector.git
    /// </remarks>
    [CustomPropertyDrawer(typeof(TagSelectorAttribute))]
    public class TagSelectorEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.String)
            {
                EditorGUI.PropertyField(position, property, label);
                return;
            }

            EditorGUI.BeginProperty(position, label, property);
            property.stringValue = EditorGUI.TagField(position, label, property.stringValue);
            EditorGUI.EndProperty();
        }
    }
}