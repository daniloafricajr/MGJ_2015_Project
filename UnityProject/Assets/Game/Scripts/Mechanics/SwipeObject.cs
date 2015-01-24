using UnityEngine;
using System.Collections;

public class SwipeObject : MonoBehaviour, IMechanic {
	public enum PointProperty
	{
		None, Complete, Fail
	}
	
	#region IMechanic implementation
	public event System.Action MechanicComplete;
	public event System.Action MechanicFailed;
	#endregion

	public Collider2D objectToSwipe;
	[HideInInspector] public Vector3 startPoint = Vector3.left;
	[HideInInspector] public Vector3 endPoint = Vector3.right;

	public PointProperty startPointComplete = PointProperty.None;
	public PointProperty endPointComplete = PointProperty.None;

	private Collider2D selectedObject;
	private Vector3 prevPos = Vector3.zero;

	void OnEnable()
	{
		Vector3 objectLocalPos = transform.InverseTransformPoint(objectToSwipe.transform.position);
		objectToSwipe.transform.position = transform.TransformPoint(GetNearestPoint(objectLocalPos, startPoint, endPoint));
		selectedObject = null;
		float zValue = transform.InverseTransformPoint(objectToSwipe.transform.position).z;
		startPoint.z = zValue;
		endPoint.z = zValue;
	}

	void Update()
	{
		if (Input.GetMouseButton(0))
		{
			Vector3 curPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			if (selectedObject == null)
			{
				RaycastHit2D rayCastHit2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
				if (rayCastHit2D.collider == objectToSwipe)
				{
					selectedObject = objectToSwipe;
				}
			}
			else
			{
				Vector3 delta = curPos - prevPos;
				Vector3 objectLocalPos = transform.InverseTransformPoint(selectedObject.transform.position);
				objectLocalPos += delta;
				objectLocalPos = GetNearestPoint(objectLocalPos, startPoint, endPoint);
				selectedObject.transform.position = transform.TransformPoint(objectLocalPos);

				if (Vector3.Distance(objectLocalPos, startPoint) < 0.01f)
				{
					if (startPointComplete == PointProperty.Fail)
					{
						if (MechanicFailed != null) MechanicFailed();
					}
					else if (startPointComplete == PointProperty.Complete)
					{
						if (MechanicComplete != null) MechanicComplete();
					}
				}
				else if (Vector3.Distance(objectLocalPos, endPoint) < 0.01f)
				{
					if (endPointComplete == PointProperty.Fail)
					{
						if (MechanicFailed != null) MechanicFailed();
					}
					else if (endPointComplete == PointProperty.Complete)
					{
						if (MechanicComplete != null) MechanicComplete();
					}
				}

			}
			prevPos = curPos;
		}
		else selectedObject = null;
	}

	private Vector3 GetNearestPoint(Vector3 objectCenter, Vector3 startPoint, Vector3 endPoint)
	{
		// assumption startPoint is origin point. And objectCenter is in localSpace
		// displace all points to startPoint Origin
		Vector3 endPointOrigin = endPoint - startPoint; 
		Vector3 objectCenterOrigin = objectCenter - startPoint;
		float scalar = Vector2.Dot (objectCenterOrigin, endPointOrigin.normalized);
		Vector3 unclampProjection = (endPointOrigin.normalized * scalar) + startPoint;
		unclampProjection.x = Mathf.Clamp(unclampProjection.x, Mathf.Min(startPoint.x, endPoint.x), Mathf.Max(startPoint.x, endPoint.x));
		unclampProjection.y = Mathf.Clamp(unclampProjection.y, Mathf.Min(startPoint.y, endPoint.y), Mathf.Max(startPoint.y, endPoint.y));
		unclampProjection.z = objectCenter.z;
		return unclampProjection;
	}
}
