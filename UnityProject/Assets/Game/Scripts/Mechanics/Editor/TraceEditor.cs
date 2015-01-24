using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(Trace))]
public class TraceEditor : Editor {

	public override void OnInspectorGUI ()
	{
		Trace targetObj = target as Trace;
		base.OnInspectorGUI();
		GUILayout.BeginVertical();
		if (GUILayout.Button("Add Point"))
		{
			if (targetObj.dotPositions.Length > 0)
			{
				ArrayUtility.Add<Vector3>(ref targetObj.dotPositions, targetObj.dotPositions[targetObj.dotPositions.Length - 1] + Vector3.right);
			}
			else 
			{
				ArrayUtility.Add<Vector3>(ref targetObj.dotPositions, Vector3.right);
			}
		}
		GUILayout.BeginVertical("box");
		int removePointIndex = -1;
		for (int i = 0; i < targetObj.dotPositions.Length; i++)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label("#" + i.ToString());
			if (GUILayout.Button("Remove Point"))
			{
				removePointIndex = i;
			}
			GUILayout.EndHorizontal();
		}
		if (removePointIndex != -1)
		{
			ArrayUtility.RemoveAt<Vector3>(ref targetObj.dotPositions, removePointIndex);
		}
		GUILayout.EndVertical();
		GUILayout.EndVertical();

		if (GUI.changed)
		{
			EditorUtility.SetDirty(target);
		}
	}

	void OnSceneGUI()
	{
		Trace targetObj = target as Trace;
		for (int i = 0; i < targetObj.dotPositions.Length; i++)
		{
			targetObj.dotPositions[i] = targetObj.transform.InverseTransformPoint(Handles.PositionHandle(targetObj.transform.TransformPoint(targetObj.dotPositions[i]), Quaternion.identity));
			Handles.Label(targetObj.transform.TransformPoint(targetObj.dotPositions[i]), "#" + i.ToString());
		}
	}
}
