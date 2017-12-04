using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour {

    [SerializeField]
    GameObject agentPrefab;

    static SceneManager instance = null;

    GameObject [] spawnPoints;

    [SerializeField]
    GameObject selectionIndicatorPrefab;
    GameObject selectionIndicator;
    GameObject selectedObject = null;

	[Header("Agent Stats Panel")]
	[SerializeField]
	GameObject agentStatsPanel;
	[SerializeField]
	Image agentEnergyBar;
	[SerializeField]
	Text agentStatsPanelInputText;
	[SerializeField]
	Text agentStatsPanelOutputText;

	// Use this for initialization
	void Start ()
    {
        if (instance == null)
            instance = this;
        else
            Debug.LogError("SCENE MANAGER IS TRYING TO BE CREATED THE SECOND TIME");

        selectionIndicator = Instantiate(selectionIndicatorPrefab, Vector3.zero, Quaternion.identity);
        HideObject(selectionIndicator, true);

		HideObject(agentStatsPanel, true);

        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
		bool first = true;
        foreach (GameObject spawnPoint in spawnPoints)
        {
            GameObject agent = SpawnAgent(spawnPoint.transform, null);
			if (first == true)
			{
				SelectAgent(agent);
				first = false;
			}
        }

        instance = this;
	}
	
	// Update is called once per frame
	void Update ()
    {
        UpdateSelectionIndicator();
	}

    void UpdateSelectionIndicator()
    {
        if (selectedObject) {
            selectionIndicator.transform.SetPositionAndRotation(
                new Vector3(selectedObject.transform.position.x, 0.0f, selectedObject.transform.position.z),
                selectedObject.transform.rotation);
        }
    }

	void OnGUI()
	{
		UpdateSelectedAgentStatsPanel();
	}

    void UpdateSelectedAgentStatsPanel()
    {
        if (selectedObject) {
			// Update energy bar
			AgentController agentController = selectedObject.GetComponent<AgentController>();
			agentEnergyBar.fillAmount = agentController.GetNormalizedEnergyLevel();

			// Update input values
			agentStatsPanelInputText.text = GetArrayToStatString(agentController.GetInputValues());
			// Update output values
			agentStatsPanelOutputText.text = GetArrayToStatString(agentController.GetOutputValues());
        }
    }

	string GetArrayToStatString(float [] array)
	{
		string statString = "";
		foreach (float value in array)
		{
			if (statString != "")
				statString += "\n";

			statString += value.ToString("n4");
		}
		return statString;
	}

    public void KillAgent(GameObject agent)
    {
		GameObject randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

		StartCoroutine(SpawnAgentAtFreeSpawnPoint(agent));
        SpawnAgent(randomSpawnPoint.transform, agent);
    }

	IEnumerator SpawnAgentAtFreeSpawnPoint(GameObject agent)
	{
		foreach (GameObject spawnPoint in spawnPoints)
		{
			bool occupied = false;
			BoxCollider boundingBox = spawnPoint.GetComponent<BoxCollider>();
			Collider [] colliders = Physics.OverlapBox(spawnPoint.transform.position + boundingBox.center,
								   boundingBox.size, spawnPoint.transform.rotation, LayerMask.NameToLayer("Agent"));
			foreach (Collider collider in colliders)
			{
				if (collider.gameObject.tag != "Agent" && collider.gameObject.tag != "Player")
					continue;

				occupied = true;
				break;
			}

			if (occupied)
			{
				SpawnAgent(spawnPoint.transform, agent);
				yield break;
			}
		}
		yield return new WaitForSeconds(.1f);
	}

    private GameObject SpawnAgent(Transform spawnObject, GameObject agent)
    {
        if (agent)
            agent.transform.SetPositionAndRotation(spawnObject.position, spawnObject.rotation);
        else
            agent = agent ?? Instantiate(agentPrefab, spawnObject.position, spawnObject.rotation) as GameObject;

        AgentController agentController = agent.GetComponent<AgentController>();
        agentController.Reset();

		return agent;
    }

    public void SelectAgent(GameObject agent)
    {
        if (selectedObject == agent)
            return;
        if (agent.tag != "Agent")
            return;

		Debug.Log(agent + " was selected");

        selectedObject = agent;

        selectionIndicator.transform.SetPositionAndRotation(
            new Vector3(agent.transform.position.x, 0.0f, agent.transform.position.z),
            agent.transform.rotation);
        HideObject(selectionIndicator, false);

		HideObject(agentStatsPanel, false);
    }

    void HideObject(GameObject gameObject, bool hide)
    {
        var renderers = gameObject.GetComponentsInChildren<Renderer>();
        foreach (var r in  renderers) {
            r.enabled = !hide;
        }
    }

    public static SceneManager Get()
    {
        return instance;
    }
}
