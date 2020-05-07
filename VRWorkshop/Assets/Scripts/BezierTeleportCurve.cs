using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierTeleportCurve : MonoBehaviour
{
    public LineRenderer lineRenderer;
    
    private int points = 500;
    private Vector3[] positions =  new Vector3[500];
    public Vector3 point0, point1,point2;
    
    bool breakLoop = false;

    public LayerMask canTeleport;
    public LayerMask cannotTeleport;

    public bool allowTeleportation = false;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer.positionCount = points;
    }

    // Update is called once per framex
    void Update()
    {
        Quaternion rot = transform.rotation;
        rot.eulerAngles = new Vector3(rot.eulerAngles.x, rot.eulerAngles.y, 0);
        transform.rotation = rot;

        DrawLinearCurve();
    }

    private void DrawLinearCurve() {
        // 3 points of the curve
        point0 = transform.position;
        point1 = transform.forward * 75.0f+ transform.position + transform.up * 75.0f;
        point2 = transform.position + transform.forward * 100.0f + transform.up * -75.0f;
        
        
        for (int i = 0; i < points; i++) {
            float t = i / (float)points;
            positions[i] = CalculateQuadraticBezierPoint(t, point0, point1,point2);
            if (i > 0) {
                if (CheckEndPoint(positions[i - 1], positions[i],i))
                {
                    lineRenderer.positionCount = i + 1;
                    breakLoop = true;
                }
            }
            if (breakLoop) break;
        }
        breakLoop = false;
        lineRenderer.SetPositions(positions);
    }

    //check if endpoint is arrived or if there is a collider between points
    private bool CheckEndPoint(Vector3 last, Vector3 current, int i) {
        RaycastHit hit;
        if (Physics.Linecast(last, current, out hit,canTeleport)) {
            point2 = hit.point;
            positions[i] = hit.point;
            lineRenderer.material.color = Color.green;
            allowTeleportation = true;
            return true;
        }

        if (Physics.Linecast(last, current, out hit, cannotTeleport))
        {
            point2 = hit.point;
            positions[i] = hit.point;
            lineRenderer.material.color = Color.red;
            allowTeleportation = false;
            return true;
        }
        return false;
    }

    private Vector3 CalculateQuadraticBezierPoint(float t, Vector3 point0, Vector3 point1, Vector3 point2) {
        // calculate intermediate points
        Vector3 point = ((1-t)*(1-t)) * point0 + 2 * (1-t) * t * point1 + (t*t) * point2;
        return point;
    }
}
