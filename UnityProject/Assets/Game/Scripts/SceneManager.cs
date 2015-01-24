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
	public Animator animationState;
	
	public event Action SceneFailed;
	public event Action SceneComplete;

	private float timeOut = 0;
	private IMechanic mechanicType;
	private ISuccessByTime successByTime;

	void OnEnable()
	{
		mechanicType = GetComponent(typeof(IMechanic)) as IMechanic;
		successByTime = mechanicType as ISuccessByTime;
		timeOut = maxTime * GameManager.totalTimePercentage;
		mechanicType.MechanicComplete += SuccessMechanic;
		mechanicType.MechanicFailed += FailMechanic;
		animationState = GetComponent<Animator>();
	}
	
	void OnDisable()
	{
		mechanicType.MechanicComplete -= SuccessMechanic;
		mechanicType.MechanicFailed -= FailMechanic;
	}

	void SuccessMechanic()
	{
		animationState.SetTrigger("Complete");
		StartCoroutine (SceneCompleteCoroutine ());
	}

	IEnumerator SceneCompleteCoroutine()
	{
		int nameHash = Animator.StringToHash("Complete");
		while (animationState.IsInTransition(0) && animationState.GetNextAnimatorStateInfo(0).nameHash == nameHash)
		{
			yield return null;
		}
		if (SceneComplete != null) SceneComplete();
	}

	IEnumerator SceneFailCoroutine()
	{
		int nameHash = Animator.StringToHash("Fail");
		while (animationState.IsInTransition(0) && animationState.GetNextAnimatorStateInfo(0).nameHash == nameHash)
		{
			yield return null;
		}
		if (SceneFailed != null) SceneFailed();
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
				if (successByTime == null)
				{
					animationState.SetTrigger("Fail");
					StartCoroutine(SceneFailCoroutine ());
				}

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
