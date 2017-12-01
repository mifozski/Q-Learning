using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AgentMotor))]
public class AgentController : MonoBehaviour {

	AgentMotor motor;

	[SerializeField]
	Light spotLight;
	float viewAngle;
	[SerializeField]
	float viewDistance = 2;
	Visibilty visibilty;

	LineRenderer lineRenderer;
	int lengthOfLineRenderer = 20;

	public Color originalSpotlightColor = new Color(34, 218, 65);

	void Start ()
	{
		motor = GetComponent<AgentMotor>();

		viewAngle = spotLight.spotAngle;

		lineRenderer = gameObject.AddComponent<LineRenderer>();
		lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
		lineRenderer.startWidth = lineRenderer.endWidth = 0.02f;
		lineRenderer.positionCount = lengthOfLineRenderer + 2;
		lineRenderer.numCornerVertices = 2;
		lineRenderer.useWorldSpace = false;

		visibilty = GetComponentInChildren<Visibilty>();

		if (gameObject.tag != "Player")
			Destroy(GetComponent<Player>());
	}
	
	void Update ()
	{
		viewAngle = spotLight.spotAngle;
		visibilty.SetVisibilityRadius(viewDistance);
		visibilty.SetViewAngle(viewAngle);

		DrawWatchingSpotlight();

		List<Transform> visibleAgents = visibilty.GetVisibleAgents();
		if (visibleAgents.Count > 0)
			spotLight.color = Color.red;
		else
			spotLight.color = originalSpotlightColor;
	}

	private void DrawWatchingSpotlight()
	{
		var points = new Vector3[lengthOfLineRenderer + 2];

		points[0] = Vector3.zero;
		float step = viewAngle / (lengthOfLineRenderer - 1);
		float startAngle = viewAngle / 2 - viewAngle;
		for (int i = 0; i < lengthOfLineRenderer; i++)
		{
			points[i + 1] = new Vector3(viewDistance * Mathf.Sin((startAngle + i * step) * Mathf.Deg2Rad), 0.0f, viewDistance * Mathf.Cos((startAngle + i * step) * Mathf.Deg2Rad));
		}

		points[lengthOfLineRenderer + 1] = Vector3.zero;

		lineRenderer.SetPositions(points);
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawRay(transform.position, transform.forward * viewDistance);

		float step = viewAngle / (lengthOfLineRenderer - 1);
		float startAngle = viewAngle / 2 - viewAngle;
		Vector3 prevPoint = transform.position;
		for (int i = 0; i < lengthOfLineRenderer; i++)
		{
			var point = transform.position + transform.rotation * new Vector3(viewDistance * Mathf.Sin((startAngle + i * step) * Mathf.Deg2Rad), 0.0f, viewDistance * Mathf.Cos((startAngle + i * step) * Mathf.Deg2Rad));
			Gizmos.DrawLine(prevPoint, point);
			prevPoint = point;
		}
	}

	public void Move(Vector3 direction)
	{
		motor.Move(direction);
	}

	public void MoveBackwards(bool move)
	{
		if (move)
			motor.MoveBackwards();
	}
}
