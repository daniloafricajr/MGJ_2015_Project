using UnityEngine;
using System.Collections;
using System;

public class SceneManager : MonoBehaviour {
	public enum Role
	{
		All, Artist, Designer, Programmer, QA
	}

	[Range(1, 60)] public int maxTime;
	[Range(1, 60)] public int timePunishmentOnFalure;
	public Role role = Role.All;
	public string sceneAction;
	
	public event Action SceneFailed;
	public event Action SceneComplete;

	private float timeOut = 0;
	private IMechanic mechanicType;
	private ISuccessByTime successByTime;
	private IMultipleExecution multipleExecution;

	void OnEnable()
	{
		mechanicType = GetComponent(typeof(IMechanic)) as IMechanic;
		multipleExecution = mechanicType as IMultipleExecution;
		successByTime = mechanicType as ISuccessByTime;
		timeOut = maxTime * GameManager.totalTimePercentage;
		mechanicType.MechanicComplete += SuccessMechanic;
		mechanicType.MechanicFailed += FailMechanic;
	}

	void OnDisable()
	{
		mechanicType.MechanicComplete -= SuccessMechanic;
		mechanicType.MechanicFailed -= FailMechanic;
	}

	void SuccessMechanic()
	{
		if (SceneComplete != null) SceneComplete();
	}

	void FailMechanic()
	{
		timeOut -= timePunishmentOnFalure;
	}

	// Update is called once per frame
	void Update () {
		if (timeOut > 0)
		{
			timeOut -= Time.deltaTime;
			if (successByTime != null) successByTime.ReportTime(timeOut);
			if (timeOut <= 0)
			{
				if (successByTime == null && SceneFailed != null) SceneFailed(); 
			}
		}
	}

	public float TimeLeft()
	{
		if (successByTime != null && timeOut <= 0)
			return maxTime * GameManager.totalTimePercentage;
		else 
			return timeOut;
	}
}
