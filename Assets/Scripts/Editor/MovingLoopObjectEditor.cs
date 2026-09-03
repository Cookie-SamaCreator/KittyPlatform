using UnityEditor;

[CustomEditor(typeof(MovingLoopObject))]
public sealed class MovingLoopObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawPropertiesExcluding(serializedObject, "speed");
        serializedObject.ApplyModifiedProperties();
    }
}
