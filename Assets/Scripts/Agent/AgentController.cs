using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface AgentControllerListener
{
	void OnKill();
}

public interface AgentHearingListener
{
	void OnEmittedNoiseLevel(Transform agent, float volume);
}

[RequireComponent(typeof(AgentMotor))]
public class AgentController : MonoBehaviour {

	AgentMotor motor;

	[Header("Sensors")]
	[SerializeField]
	Light spotLight;
	[SerializeField]
	float viewAngle;
	[SerializeField]
	float viewDistance = 2;
	AgentVisibilty visibilty;
	public Color originalSpotlightColor = new Color(34, 218, 65);

	[SerializeField]
	float hearingRadius = 5;
	AgentHearing hearing;

	LineRenderer lineRenderer;
	int lengthOfLineRenderer = 20;

	[Header("Unity Stuff")]
	[SerializeField]
	Image energyBar;

	public AgentInternalState internalState = new AgentInternalState();
	private AgentSettings settings = new AgentSettings();

	AgentBrains brains;

	List<AgentControllerListener> listeners = new List<AgentControllerListener>();
	List<AgentHearingListener> hearingListeners = new List<AgentHearingListener>();

	void Start ()
	{
		motor = GetComponent<AgentMotor>();

		visibilty = GetComponentInChildren<AgentVisibilty>();

		hearing = GetComponentInChildren<AgentHearing>();

		viewAngle = spotLight.spotAngle;

		lineRenderer = gameObject.AddComponent<LineRenderer>();
		lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
		lineRenderer.startWidth = lineRenderer.endWidth = 0.02f;
		lineRenderer.positionCount = lengthOfLineRenderer + 2;
		lineRenderer.numCornerVertices = 2;
		lineRenderer.useWorldSpace = false;

		if (gameObject.tag != "Player")
		{
			Destroy(GetComponent<Player>());

			brains = GetComponent<AgentBrains>();
		}
		else
			Destroy(GetComponent<AgentBrains>());
	}
	
	void Update ()
	{
		// 'Kill' agent if the energy is depleted
		if (internalState.energy == 0.0f && tag == "Agent")
		{
			foreach (AgentControllerListener listener in listeners)
				listener.OnKill();

			SceneManager.Get().KillAgent(gameObject);
		}

		EmitNoise();
		
		viewAngle = spotLight.spotAngle;
		visibilty.SetVisibilityRadius(viewDistance);
		visibilty.SetViewAngle(viewAngle);

		hearing.SetHearingRadius(hearingRadius);

		DrawVisibilitySpotlight();

		List<Transform> visibleAgents = visibilty.GetVisibleAgents();
		if (visibleAgents.Count > 0)
			spotLight.color = Color.red;
		else
			spotLight.color = originalSpotlightColor;

		DepleteEnergy();
	}

	void DepleteEnergy()
	{
		internalState.energy = Mathf.Max(internalState.energy - Time.deltaTime * settings.energyDepletionSpeed, 0.0f);
		// energyBar.fillAmount = internalState.energy / 100.0f;
	}

	void EmitNoise()
	{
		foreach (AgentHearingListener listener in hearingListeners)
		{
			listener.OnEmittedNoiseLevel(transform, 0.5f * Time.deltaTime);
		}
	}

	void OnGui()
	{
		Vector2 targetPos;
 		targetPos = Camera.main.WorldToScreenPoint(transform.position);
       
		GUI.Box(new Rect(targetPos.x, Screen.height - 50, 60, 20), 10 + "/" + 20);
	}

	private void DrawVisibilitySpotlight()
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

	public void Move(Vector2 direction)
	{
		motor.Move(direction);
	}

	public void MoveBackwards(bool move)
	{
		if (move)
			motor.MoveBackwards();
	}

	public float [] GetInputValues()
	{
		if (tag == "Agent")
			return brains.GetLastInputs();
		else
			return new float [] {};
	}

	public float [] GetOutputValues()
	{
		if (tag == "Agent")
			return brains.GetLastOuputs();
		else
			return new float [] {};
	}

	public float GetNormalizedEnergyLevel()
	{
		return internalState.energy / 100.0f;
	}

	public int GetVisibleEnemiesNum()
	{
		return visibilty.GetVisibleAgentNum();
	}

	public float GetHeardNoiseLevel()
	{
		return hearing.GetHeardNoiseLevel();
	}

	public float GetNormalizedAngleToNearestAgent()
	{
		return visibilty.GetNormalizedAngleToNearestAgent();
	}

	public AgentInternalState GetInternalState()
	{
		return internalState;
	}

	public void Reset()
	{
		internalState.Reset();
	}

	public void AddHearingListener(AgentHearingListener listener)
	{
		if (hearingListeners.Contains(listener) == false)
			hearingListeners.Add(listener);
	}

	public void RemoveHearingListener(AgentHearingListener listener)
	{
		hearingListeners.Remove(listener);
	}
}
