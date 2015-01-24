using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(SwipeObject))]
public class SwipeObjectEditor : Editor {
	
	void OnSceneGUI()
	{
		SwipeObject targetObj = target as SwipeObject;
		targetObj.startPoint = targetObj.transform.TransformPoint(Handles.PositionHandle(targetObj.transform.InverseTransformPoint(targetObj.startPoint), Quaternion.identity));
		targetObj.endPoint = targetObj.transform.TransformPoint(Handles.PositionHandle(targetObj.transform.InverseTransformPoint(targetObj.endPoint), Quaternion.identity));
		Handles.DrawLine(targetObj.transform.InverseTransformPoint(targetObj.startPoint), targetObj.transform.InverseTransformPoint(targetObj.endPoint));
		Handles.Label(targetObj.transform.InverseTransformPoint(targetObj.startPoint), "Start Point");
		Handles.Label(targetObj.transform.InverseTransformPoint(targetObj.endPoint), "End Point");
	}
}
