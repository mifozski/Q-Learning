using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AgentMotor))]
public class Player : MonoBehaviour {

    AgentMotor agentMotor;

    float angle;
    float smoothInputMagniture;
    float smoothMoveVelocity;

    // Use this for initialization
    void Start () {
        agentMotor = GetComponent<AgentMotor>();

        angle = Vector3.Angle(Vector3.forward, transform.forward);
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
        float inputMagnitude = inputDirection.magnitude;
        smoothInputMagniture = Mathf.SmoothDamp(smoothInputMagniture, inputMagnitude, ref smoothMoveVelocity, agentMotor.smoothMoveTime);

        float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
        angle = Mathf.LerpAngle(angle, targetAngle, Time.deltaTime * agentMotor.turnSpeed * inputMagnitude);
        transform.eulerAngles = Vector3.up * angle;

        float moveSpeed = agentMotor.moveSpeed;

        transform.Translate(transform.forward * moveSpeed * Time.deltaTime * smoothInputMagniture, Space.World);
	}
}