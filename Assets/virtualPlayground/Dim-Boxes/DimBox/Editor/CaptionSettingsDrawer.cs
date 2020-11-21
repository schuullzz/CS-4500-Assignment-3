using UnityEngine;
using UnityEditor;
using System.Collections;

namespace DimBoxes
{
    [CustomPropertyDrawer(typeof(DimBox.CaptionSettings))]
    class CaptionSettingsDrawer : PropertyDrawer
    {
        //public bool showProperty = true;
        float height = 0f;
        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            height = 0f;
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property. 
            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            //property.isExpanded = EditorGUI.Foldout(new Rect(position.x, position.y + 3, position.width - 6, 15), property.isExpanded, label,  EditorStyles.foldout);
            //height += 20;
            //public static bool Foldout(Rect position, bool foldout, string content, GUIStyle style = EditorStyles.foldout);
            //position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            if (property.isExpanded)
            {
                var indent = EditorGUI.indentLevel;
                EditorGUI.indentLevel = indent + 1;

                // Calculate rects
                DimBox db = property.serializedObject.targetObject as DimBox;
                //Rect boolRect = new Rect(position.x, position.y + height, position.width, 20); height += 20;
                bool isOn = db.formatter.captionMode;
                if (isOn)
                {
                    Rect hRect = new Rect(position.x, position.y + height, position.width, 20); height += 20;
                    Rect dRect = new Rect(position.x, position.y + height, position.width, 20); height += 20;
                    Rect wRect = new Rect(position.x, position.y + height, position.width, 20); height += 20;
                
                // Draw fields - passs GUIContent.none to each so they are drawn without labels
                SerializedProperty m_h_text = property.FindPropertyRelative("heightCaption");
                //SerializedProperty m_h_flip = property.FindPropertyRelative("h_flip");
                SerializedProperty m_d_text = property.FindPropertyRelative("depthCaption");
                //SerializedProperty m_d_flip = property.FindPropertyRelative("d_flip");
                SerializedProperty m_w_text = property.FindPropertyRelative("widthCaption");
                //SerializedProperty m_w_flip = property.FindPropertyRelative("w_flip");

                    EditorGUI.PropertyField(hRect, m_h_text, new GUIContent("heightCaption"));
                    EditorGUI.PropertyField(dRect, m_d_text, new GUIContent("depthCaption"));
                    EditorGUI.PropertyField(wRect, m_w_text, new GUIContent("heightCaption"));
                }

                // Set indent back to what it was
                EditorGUI.indentLevel = indent;
            }

            EditorGUI.EndProperty();
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return height;
        }

    }
}
