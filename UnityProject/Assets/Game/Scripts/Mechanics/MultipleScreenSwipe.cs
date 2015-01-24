using UnityEngine;
using System.Collections;
using System;

public class MultipleScreenSwipe : MonoBehaviour, IMechanic, IMultipleExecution {
	
	#region IMechanic implementation
	public event Action MechanicComplete;
	public event Action MechanicFailed;
	#endregion

	#region IMultipleExecution implementation
	public event Action<int> TimesExecutedChanged;
	#endregion

	public SwipeDetection.DetectedType successSwipe = SwipeDetection.DetectedType.Up;
	public bool isStrictSwipe = false;
	[Range(0, 60)] public int timesToSwipe;

	private int completedCount = 0;

	void OnEnable()
	{
		completedCount = 0;
	}

	void Update()
	{
		SwipeDetection.DetectedType detectedSwipe = SwipeDetection.DetectSwipe();
		if (detectedSwipe == successSwipe)
		{
			if (timesToSwipe != 0 && ++completedCount >= timesToSwipe)
			{
				if (MechanicComplete != null) MechanicComplete();
			}
			else
			{
				if (TimesExecutedChanged != null) TimesExecutedChanged(completedCount);
			}
		}
		else if (isStrictSwipe && detectedSwipe != SwipeDetection.DetectedType.None)
		{
			if (MechanicFailed != null) MechanicFailed();
		}
		
	}
}
