using RaccoonNinjaToolbox.Scripts.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace RaccoonNinjaToolbox.Scripts.Editor
{
    [CustomPropertyDrawer(typeof(TypedAudioClip))]
    public class TypedAudioClipPropertyDrawer : PropertyDrawer
    {
        private const float ButtonWidth = 24f;
        private const float ButtonSpacing = 2f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, label);

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var buttonsWidth = (ButtonWidth + ButtonSpacing) * 2;
            var fieldRect = new Rect(position.x, position.y, position.width - buttonsWidth, position.height);
            var playRect = new Rect(fieldRect.xMax + ButtonSpacing, position.y, ButtonWidth, position.height);
            var editRect = new Rect(playRect.xMax + ButtonSpacing, position.y, ButtonWidth, position.height);

            EditorGUI.ObjectField(fieldRect, property, GUIContent.none);

            var typedClip = property.objectReferenceValue as TypedAudioClip;

            using (new EditorGUI.DisabledScope(!typedClip))
            {
                if (GUI.Button(playRect, new GUIContent("\u25b6", "Preview with randomized volume/pitch")))
                    TypedAudioClipPreview.PlayPreview(typedClip);

                if (GUI.Button(editRect, new GUIContent("\u2026", "Select in inspector")))
                {
                    EditorGUIUtility.PingObject(typedClip);
                    Selection.activeObject = typedClip;
                }
            }

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}
