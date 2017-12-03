using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButtonDown(0) == false)
			return;

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		var hitInfos = Physics.RaycastAll(ray, LayerMask.NameToLayer("Selectable"));
		foreach (var hitInfo in hitInfos)
		{
			if (hitInfo.collider.gameObject.layer != LayerMask.NameToLayer("Selectable"))
				continue;

			GameObject hitObject = hitInfo.collider.gameObject;

			SceneManager.Get().SelectAgent(hitObject);

			break;
		}
	}
}
