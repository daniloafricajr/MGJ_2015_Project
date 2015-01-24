using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultipleChoice : MonoBehaviour, IMechanic, IMultipleExecution {
	[System.Serializable]
	public class Item
	{
		public bool isFailure = false;
		public Collider2D itemToChoose;
	}

	#region IMechanic implementation
	public event System.Action MechanicComplete;
	public event System.Action MechanicFailed;
	#endregion

	#region IMultipleExecution implementation
	public event System.Action<int> TimesExecutedChanged;
	#endregion
	
	public Item[] choices = new Item[0];
	private List<Collider2D> correctItemList;

	void OnEnable()
	{
		correctItemList = new List<Collider2D>();
		for (int i = 0; i < choices.Length; i++)
		{
			if (!choices[i].isFailure) correctItemList.Add(choices[i].itemToChoose);
		}
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit2D hit2d = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.zero);
			for (int i = 0; i < choices.Length; i++)
			{
				if (choices[i].itemToChoose == hit2d.collider)
				{
					if (choices[i].isFailure)
					{
						if (MechanicFailed != null) MechanicFailed();
						break;
					}
					else if (correctItemList.Count > 0 && correctItemList.Contains(choices[i].itemToChoose))
					{
						correctItemList.Remove(choices[i].itemToChoose);
						if (correctItemList.Count > 0)
						{
							if (TimesExecutedChanged != null) TimesExecutedChanged(i);
						}
						else
						{
							if (MechanicComplete != null) MechanicComplete();
						}
					}
				}
			}
		}
	}
}
