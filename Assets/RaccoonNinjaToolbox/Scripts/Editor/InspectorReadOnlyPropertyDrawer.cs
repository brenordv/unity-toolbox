using RaccoonNinjaToolbox.Scripts.Attributes;
using UnityEditor;
using UnityEngine;

namespace RaccoonNinjaToolbox.Scripts.Editor
{
    [CustomPropertyDrawer(typeof(InspectorReadOnlyAttribute), true)]
    public class InspectorReadOnlyPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label);
            GUI.enabled = true;
        }
    }
}