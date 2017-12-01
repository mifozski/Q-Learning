using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMotor : MonoBehaviour {

    public float moveSpeed = 7f;
    public float smoothMoveTime = .1f;
    public float turnSpeed = 8;

    float angle;
    float smoothInputMagniture;
    float smoothMoveVelocity;
    Vector3 velocity;

    Rigidbody rigidBody;

	void Start ()
    {
        angle = Vector3.Angle(Vector3.forward, transform.forward);

        rigidBody = GetComponent<Rigidbody>();
    }
	
	void Update ()
    {
        velocity = transform.forward * moveSpeed * smoothInputMagniture;
    }

    void FixedUpdate()
    {
        rigidBody.MoveRotation(Quaternion.Euler(Vector3.up * angle));
        rigidBody.MovePosition(rigidBody.position + velocity * Time.deltaTime);
    }

	public void Move(Vector3 direction)
	{
		direction = direction.normalized;
		float inputMagnitude = direction.magnitude;
		smoothInputMagniture = Mathf.SmoothDamp(smoothInputMagniture, inputMagnitude, ref smoothMoveVelocity, smoothMoveTime);

		float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
		angle = Mathf.LerpAngle(angle, targetAngle, Time.deltaTime * turnSpeed * inputMagnitude);
		transform.eulerAngles = Vector3.up * angle;

		transform.Translate(transform.forward * moveSpeed * Time.deltaTime * smoothInputMagniture, Space.World);
	}

	public void MoveBackwards()
	{
		transform.Translate(-transform.forward * moveSpeed * Time.deltaTime, Space.World);
	}
}