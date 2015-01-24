using UnityEngine;
using System.Collections;

public class SwipeDetection {
	public enum DetectedType
	{
		None, Up, Down, Left, Right
	}

	private static Vector3 prevPos;

	// use this on Update Loop!!!
	public static DetectedType DetectSwipe()
	{
		Vector3 curPos = Input.mousePosition;
		DetectedType retVal = DetectedType.None;

		if (Input.GetMouseButton(0) && Vector3.Distance(curPos, prevPos) > 0)
		{
			Vector3 delta = curPos - prevPos;
			delta.Normalize();
			float deg = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
			if (deg < 45.0f && deg > -45.0f) retVal = DetectedType.Left;
			else if (deg > 45.0f && deg < 135.0f) retVal = DetectedType.Up;
			else if (deg > 135.0f || deg < -135.0f) retVal = DetectedType.Right;
			else if (deg > -135.0f && deg < -45.0f) retVal = DetectedType.Down;
			else retVal = DetectedType.None;
		}

		prevPos = curPos;
		return retVal;
	}

}
