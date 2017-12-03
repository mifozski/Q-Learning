using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentHearing : MonoBehaviour, AgentHearingListener {

    SphereCollider hearingCollider;

	List<Transform> hearableAgents = new List<Transform>();

	[SerializeField]
	float heardNoiseLevelSum = 0.0f;

    void Start ()
    {
		hearingCollider = gameObject.AddComponent<SphereCollider>();
        hearingCollider.isTrigger = true;
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag != "Agent" && other.gameObject.tag != "Player")
			return;

		hearableAgents.Add(other.transform);

		AgentController agentController = other.gameObject.GetComponent<AgentController>();
		agentController.AddHearingListener(this);
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag != "Agent" && other.gameObject.tag != "Player")
			return;

		AgentController agentController = other.gameObject.GetComponent<AgentController>();
		agentController.RemoveHearingListener(this);
	}

	public float GetHeardNoiseLevel()
	{
		float totalNoiseLevel = heardNoiseLevelSum;
		heardNoiseLevelSum = 0.0f;
		return totalNoiseLevel;
	}

    public void SetHearingRadius(float radius)
    {
        hearingCollider.radius = radius;
    }

	public void OnEmittedNoiseLevel(Transform agent, float volume)
	{
		var distance = (agent.position - transform.position).magnitude;
		
		var heardLevel = volume * 1.0f / Mathf.Pow(distance, 2);
		heardNoiseLevelSum += heardLevel;
	}
}

