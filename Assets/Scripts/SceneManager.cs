using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour {

    [SerializeField]
    GameObject agentPrefab;

    static SceneManager instance = null;

    GameObject [] spawnPoints;

	// Use this for initialization
	void Start ()
    {
        if (instance == null)
            instance = this;
        else
            Debug.LogError("SCENE MANAGER IS TRYING TO BE CREATED THE SECOND TIME");

        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        foreach (GameObject spawnPoint in spawnPoints)
        {
            SpawnAgent(spawnPoint.transform, null);
        }

        instance = this;
	}
	
	// Update is called once per frame
	void Update ()
    {
	}

    public void KillAgent(GameObject agent)
    {
		GameObject randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        SpawnAgent(randomSpawnPoint.transform, agent);
    }

    private void SpawnAgent(Transform spawnObject, GameObject agent)
    {
        if (agent)
            agent.transform.SetPositionAndRotation(spawnObject.position, spawnObject.rotation);
        else
            agent = agent ?? Instantiate(agentPrefab, spawnObject.position, spawnObject.rotation) as GameObject;

        AgentController agentController = agent.GetComponent<AgentController>();
        agentController.Reset();
    }

    public static SceneManager Get()
    {
        return instance;
    }
}
