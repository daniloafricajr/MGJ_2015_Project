using UnityEngine;
using System.Collections;

public class TapObject : MonoBehaviour, IMechanic, IMultipleExecution {
	#region IMechanic implementation
	public event System.Action MechanicComplete;
	public event System.Action MechanicFailed;
	#endregion

	#region IMultipleExecution implementation
	public event System.Action<int> TimesExecutedChanged;
	#endregion

	public Collider2D objectToTap;
	[Range(0, 60)] public int timesToTap;
	private int completedTap;

	void OnEnable()
	{
		completedTap = 0;
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit2D hit2d = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.zero);
			if (hit2d.collider == objectToTap)
			{
				if (timesToTap != 0 && ++completedTap >= timesToTap)
				{
					if (MechanicComplete != null) MechanicComplete();
				}
				else
				{
					if (TimesExecutedChanged != null) TimesExecutedChanged(completedTap);
				}
			}
		}
	}
}
