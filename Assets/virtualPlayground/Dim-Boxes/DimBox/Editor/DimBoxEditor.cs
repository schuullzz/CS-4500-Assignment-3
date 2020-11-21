using UnityEngine;
using UnityEditor;


namespace DimBoxes
{
    [CustomEditor(typeof(DimBox))]
    public class DimBoxEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DimBox dimboxScript = (DimBox)target;
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Recalculate Bounds"))
            {
                dimboxScript.AccurateBounds();
                //to update with OnEnable when using threading
                //dimboxScript.enabled = false;
                //dimboxScript.enabled = true;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }

    [CustomEditor(typeof(DimBoxProgressive))]
    public class DimBoxEditorProgressive : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DimBox dimboxScript = (DimBoxProgressive)target;
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Recalculate Bounds"))
            {
                dimboxScript.AccurateBounds();
                //to update with OnEnable when using threading
                dimboxScript.enabled = false;
                dimboxScript.enabled = true;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }
}