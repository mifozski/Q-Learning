using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visibilty : MonoBehaviour {

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
		if (IsAgentVisible(agentTransform))
		{
			if (visibleAgents.Contains(agentTransform) == false)
			{
				Debug.Log(gameObject.ToString() + " sees " + other.gameObject.ToString());
				visibleAgents.Add(agentTransform);
			}
		}
		else
		{
			visibleAgents.Remove(agentTransform);
		}
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

    public void SetVisibilityRadius(float radius)
    {
        visibiltyCollider.radius = radius;
    }

	public void SetViewAngle(float angle)
	{
		viewAngle = angle;
	}
}
