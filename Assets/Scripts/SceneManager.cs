using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour {

    [SerializeField]
    GameObject agentPrefab;

	// Use this for initialization
	void Start ()
    {
        var spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        foreach (GameObject spawnPoint in spawnPoints)
        {
            GameObject agent = Instantiate(agentPrefab, transform.position, transform.rotation) as GameObject;
            Destroy(agent.GetComponent<Player>());
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
