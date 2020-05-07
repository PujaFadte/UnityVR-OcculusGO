using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public GameObject go;
    public GameObject cam;
    public LayerMask layer;
    public BezierTeleportCurve bezierCurve;
    public GameObject player;

    public LineRenderer lineRenderer;
    public GameObject goController;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position + transform.forward * 10.0f);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        transform.rotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote);

        lineRenderer.SetPosition(0, goController.transform.position);
        lineRenderer.SetPosition(1, goController.transform.position + goController.transform.forward * 50.0f);

        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger)) {
            Ray ray1 = new Ray(goController.transform.position, goController.transform.forward);
            if (Physics.Raycast(ray1, out hit, Mathf.Infinity, layer))
            {
                if (hit.collider != null && hit.collider.tag == "Agents")
                {
                    if (go != hit.collider.gameObject)
                    {
                        go = hit.collider.transform.gameObject;
                        go.gameObject.GetComponent<Renderer>().material.color = Color.red;
                        //go.GetComponent<AgentMovement>().health -= 10;
                        StartCoroutine("Delay");
                    }

                }
            }
        }

        if (OVRInput.Get(OVRInput.Button.PrimaryTouchpad) && bezierCurve.allowTeleportation) {
            Vector3 teleportPos = bezierCurve.lineRenderer.GetPosition(bezierCurve.lineRenderer.positionCount - 1);
            teleportPos.y = 1.65f;
            player.transform.position = teleportPos;
        }
    }

    IEnumerator Delay() {
        yield return new WaitForSeconds(0.5f);
        if (go != null) {
            go.GetComponent<Renderer>().material.color = Color.grey;
            go = null;
        }      
    }
}
