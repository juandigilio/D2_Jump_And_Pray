using UnityEngine;
using UnityEditor;

namespace TrueTrackSystem
{

    [CustomEditor(typeof(SplineManager))]
    public class SplineManagerEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("Drag your track prefabs into the area below. Ensure they are in the correct order. Track prefabs must have splines correctly set up to work. For example, all splines use the same tangent mode, and tangent length(value)", MessageType.Info);

            DrawDefaultInspector();

            SplineManager splineManager = (SplineManager)target;

            if (GUILayout.Button("Combine Splines"))
            {
                splineManager.CreateConnectedSpline();
            }
        }
    }
}
