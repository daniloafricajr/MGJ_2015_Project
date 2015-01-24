using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Trace : MonoBehaviour, IMechanic {
	#region IMechanic implementation
	public event Action MechanicComplete;
	public event Action MechanicFailed;
	#endregion

	public GameObject linePrefab;
	public GameObject dotPrefab;

	public Vector3[] dotPositions = new Vector3[0];
	public float lineScale = 1;
	private List<GameObject> finishedLines = new List<GameObject>();
	private List<GameObject> finishedDots = new List<GameObject>();
	private List<GameObject> dotInstance = new List<GameObject>();
	private GameObject lineInstnace;
	private GameObject finishDotCandidate;

	void OnEnable()
	{
		while (finishedLines.Count > 0)
		{
			Destroy(finishedLines[0]);
			finishedLines.RemoveAt(0);
		}
		finishedDots.Clear();
		while (dotInstance.Count > 0)
		{
			Destroy(dotInstance[0]);
			dotInstance.RemoveAt(0);
		}
		for (int i = 0; i < dotPositions.Length; i++)
		{
			GameObject dotInst = Instantiate(dotPrefab, transform.InverseTransformPoint(dotPositions[i]), Quaternion.identity) as GameObject;
			dotInst.transform.parent = transform;
			dotInstance.Add(dotInst);
		}
		if (lineInstnace != null)
			Destroy(lineInstnace);
		finishDotCandidate = null;
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit2D hit2d = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.zero);
			if (hit2d.collider != null && !finishedDots.Contains(hit2d.collider.gameObject) && dotInstance.Contains(hit2d.collider.gameObject))
			{
				lineInstnace = Instantiate(linePrefab, hit2d.collider.transform.position, Quaternion.identity) as GameObject;
				lineInstnace.transform.parent = transform;
				finishDotCandidate = hit2d.collider.gameObject;
			}
		}
		else if (lineInstnace != null && Input.GetMouseButton(0))
		{
			Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			worldMousePos.z = lineInstnace.transform.position.z;
			Vector3 direction = worldMousePos - lineInstnace.transform.position;
			Vector3 scale = new Vector3(direction.magnitude * lineScale, 1, 1);
			lineInstnace.transform.localScale = scale;
			lineInstnace.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

			RaycastHit2D hit2d = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.zero);
			if (hit2d.collider != null && hit2d.collider.gameObject != finishDotCandidate && !finishedDots.Contains(hit2d.collider.gameObject) && dotInstance.Contains(hit2d.collider.gameObject))
			{
				if (dotInstance[finishedDots.Count + 1] == hit2d.collider.gameObject)
				{
					direction = hit2d.collider.transform.position - lineInstnace.transform.position;
					scale = new Vector3(direction.magnitude * lineScale, 1, 1);
					lineInstnace.transform.localScale = scale;
					lineInstnace.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
					finishedLines.Add(lineInstnace);
					lineInstnace = null;
					finishedDots.Add(finishDotCandidate);
					finishDotCandidate = null;

					if (finishedDots.Count == dotPositions.Length - 1)
					{
						if (MechanicComplete != null) MechanicComplete();
					}
					else
					{
						lineInstnace = Instantiate(linePrefab, hit2d.collider.transform.position, Quaternion.identity) as GameObject;
						lineInstnace.transform.parent = transform;
						finishDotCandidate = hit2d.collider.gameObject;
					}
				}
				else
				{
					Destroy(lineInstnace);
					lineInstnace = null;
					finishDotCandidate = null;
					if (MechanicFailed != null) MechanicFailed();
				}
			}
		}
		else if (lineInstnace != null)
		{
			Destroy(lineInstnace);
			lineInstnace = null;
			finishDotCandidate = null;
		}

	}
}
