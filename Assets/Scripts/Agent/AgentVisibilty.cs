using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentVisibilty : MonoBehaviour {

    SphereCollider visibiltyCollider;

	float viewAngle;

	List<Transform> visibleAgents = new List<Transform>();

	[SerializeField]
	LayerMask viewMask;

    void Start ()
    {
		viewAngle = 0.0f;

		visibiltyCollider = gameObject.AddComponent<SphereCollider>();
        visibiltyCollider.isTrigger = true;
	}
	
	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag != "Agent" && other.gameObject.tag != "Player")
			return;

		Transform agentTransform = other.gameObject.transform;
		bool updateList = false;
		if (IsAgentVisible(agentTransform))
		{
			if (visibleAgents.Contains(agentTransform) == false)
			{
				// Debug.Log(gameObject.ToString() + " sees " + other.gameObject.ToString());
				visibleAgents.Add(agentTransform);
				
				updateList = true;
			}
		}
		else
		{
			visibleAgents.Remove(agentTransform);
			updateList = true;
		}

		if (updateList == true)
		{
			SortAgents();
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (visibleAgents.Contains(other.gameObject.transform))
		{
			visibleAgents.Remove(other.gameObject.transform);
			SortAgents();
		}
	}

	void SortAgents()
	{
		visibleAgents.Sort((a, b) =>
			(a.transform.position - transform.position).magnitude.CompareTo(
			b.transform.position - transform.position));
	}

	public List<Transform> GetVisibleAgents()
    {
		return visibleAgents;
    }

	public int GetVisibleAgentNum()
	{
		return visibleAgents.Count;
	}

    bool IsAgentVisible(Transform agent)
    {
        if (agent == null)
            return false;

        if (Vector3.Distance(transform.position, agent.position) < visibiltyCollider.radius)
        {
			Vector3 dirToAgent = (agent.position - transform.position).normalized;
			float angleBetweenSelfAndAgent = Vector3.Angle(transform.forward, dirToAgent);
			if (angleBetweenSelfAndAgent < viewAngle / 2.0f)
			{
				if (!Physics.Linecast(transform.position, agent.position, viewMask))
				{
					return true;
				}
			}
        }
		return false;
    }

	public float GetNormalizedAngleToNearestAgent()
	{
		if (visibleAgents.Count == 0)
			return 0.0f;

		Transform nearestAgent = visibleAgents[0];

		float angle = Vector3.Angle(transform.forward, nearestAgent.position - transform.position);
    	var cross = Vector3.Cross(transform.forward, nearestAgent.position - transform.position);
    	if (cross.y < 0) angle = -angle;

		float normalizedAngle = (angle / (viewAngle / 2.0f) + 1.0f) / 2.0f;
		
		return normalizedAngle;
	}

    public void SetVisibilityRadius(float radius)
    {
        visibiltyCollider.radius = radius;
    }

	public void SetViewAngle(float angle)
	{
		viewAngle = angle;
	}
}
