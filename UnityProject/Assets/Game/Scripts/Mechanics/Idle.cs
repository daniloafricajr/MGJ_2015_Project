using UnityEngine;
using System.Collections;

public class Idle : MonoBehaviour, IMechanic, ISuccessByTime {
	#region IMechanic implementation
	public event System.Action MechanicComplete;
	public event System.Action MechanicFailed;
	#endregion

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (MechanicFailed != null) MechanicFailed();
		}
	}

	#region ISuccessByTime implementation
	public void ReportTime (float time)
	{
		if (time <= 0)
		{
			if (MechanicComplete != null) MechanicComplete();
		}
	}
	#endregion
}
