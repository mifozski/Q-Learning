using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AgentController))]
public class Player : MonoBehaviour {

	AgentController agentController;

    // Use this for initialization
		void Start () {
			agentController = GetComponent<AgentController>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (Input.GetButton("Backwards"))
		{
			agentController.MoveBackwards(true);
			return;
		}

		agentController.Move(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));

		Camera.main.transform.position = new Vector3(transform.position.x, Camera.main.transform.position.y, transform.position.z);
	}
}