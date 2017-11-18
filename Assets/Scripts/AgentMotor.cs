using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMotor : MonoBehaviour {

    public float moveSpeed = 7f;
    public float smoothMoveTime = .1f;
    public float turnSpeed = 8;

    LineRenderer lineRenderer;
    int lengthOfLineRenderer = 20;

    [SerializeField]
    Light spotLight;
    float viewAngle;
    [SerializeField]
    float viewDistance = 2;

    [SerializeField]
    GameObject arrow;

	// Use this for initialization
	void Start () {
        viewAngle = spotLight.spotAngle;

        Debug.LogFormat("viewAngle: " + viewAngle);

        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        lineRenderer.startWidth = lineRenderer.endWidth = 0.02f;
        lineRenderer.positionCount = lengthOfLineRenderer + 2;
        lineRenderer.numCornerVertices = 2;
        lineRenderer.useWorldSpace = false;

        // spotLight.cullingMask

        var points = new Vector3[lengthOfLineRenderer + 2];

        points[0] = Vector3.zero;
        float step = viewAngle / (lengthOfLineRenderer - 1);
        float startAngle = viewAngle / 2 - viewAngle;
        for (int i = 0; i < lengthOfLineRenderer; i++)
        {
            Debug.Log("angle: " + (startAngle + i * step));
            points[i + 1] = new Vector3(viewDistance * Mathf.Sin((startAngle + i * step) * Mathf.Deg2Rad), 0.0f, viewDistance * Mathf.Cos((startAngle + i * step) * Mathf.Deg2Rad));
            Debug.Log("i: " + i + " point: " + points[i + 1]);
        }

        points[lengthOfLineRenderer + 1] = Vector3.zero;
    }
	
	// Update is called once per frame
	void Update ()
    {
        DrawWatchingSpotlight();
    }

    private void DrawWatchingSpotlight()
    {
        var points = new Vector3[lengthOfLineRenderer + 2];

        points[0] = Vector3.zero;
        float step = viewAngle / (lengthOfLineRenderer - 1);
        float startAngle = viewAngle / 2 - viewAngle;
        for (int i = 0; i < lengthOfLineRenderer; i++)
        {
            points[i + 1] = new Vector3(viewDistance * Mathf.Sin((startAngle + i * step) * Mathf.Deg2Rad), 0.0f, viewDistance * Mathf.Cos((startAngle + i * step) * Mathf.Deg2Rad));
        }

        points[lengthOfLineRenderer + 1] = Vector3.zero;

        lineRenderer.SetPositions(points);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * viewDistance);
        
        float step = viewAngle / (lengthOfLineRenderer - 1);
        float startAngle = viewAngle / 2 - viewAngle;
        Vector3 prevPoint = transform.position;
        for (int i = 0; i < lengthOfLineRenderer; i++)
        {
            var point = new Vector3(viewDistance * Mathf.Sin((startAngle + i * step) * Mathf.Deg2Rad), 0.0f, viewDistance * Mathf.Cos((startAngle + i * step) * Mathf.Deg2Rad));
            Gizmos.DrawLine(prevPoint, point);
            prevPoint = point;
        }
    }
}
