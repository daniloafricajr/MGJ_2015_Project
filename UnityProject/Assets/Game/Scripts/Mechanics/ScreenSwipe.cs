using UnityEngine;
using System;
using System.Collections;

public class ScreenSwipe : MonoBehaviour, IMechanic {
	
	#region IMechanic implementation
	public event Action MechanicComplete;
	public event Action MechanicFailed;
	#endregion

	public SwipeDetection.DetectedType successSwipe = SwipeDetection.DetectedType.Up;
	public bool isStrictSwipe = false;

	void Update()
	{
		SwipeDetection.DetectedType detectedSwipe = SwipeDetection.DetectSwipe();
		if (detectedSwipe == successSwipe)
		{
			if (MechanicComplete != null) MechanicComplete();
		}
		else if (isStrictSwipe && detectedSwipe != SwipeDetection.DetectedType.None)
		{
			if (MechanicFailed != null) MechanicFailed();
		}

	}
}
