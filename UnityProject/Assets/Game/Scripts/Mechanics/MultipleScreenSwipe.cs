using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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

	private List<int> swipeLayerIndices = new List<int>();

	void OnEnable()
	{
		swipeLayerIndices.Clear();
		for (int i = 0; i < timesToSwipe; i++)
		{
			swipeLayerIndices.Add(i);
		}
	}

	void Update()
	{
		SwipeDetection.DetectedType detectedSwipe = SwipeDetection.DetectSwipe();
		if (detectedSwipe == successSwipe)
		{
			if (swipeLayerIndices.Count > 0)
			{
				int randIndex = UnityEngine.Random.Range(0, swipeLayerIndices.Count);
				if (TimesExecutedChanged != null) TimesExecutedChanged(swipeLayerIndices[randIndex]);
				swipeLayerIndices.RemoveAt(randIndex);
			}
			if (timesToSwipe != 0 && swipeLayerIndices.Count == 0)
			{
				if (MechanicComplete != null) MechanicComplete();
			}
		}
		else if (isStrictSwipe && detectedSwipe != SwipeDetection.DetectedType.None)
		{
			if (MechanicFailed != null) MechanicFailed();
		}
		
	}
}
