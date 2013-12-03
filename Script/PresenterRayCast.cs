using UnityEngine;
using System.Collections;

public class PresenterRayCast : MonoBehaviour {

	public Camera cameraM;
	public Camera cameraA;
	public Camera cameraB;
	
	// Use this for initialization
	void Start () {
		cameraM.enabled = true;
		cameraA.enabled = false;
		cameraB.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) {
                if (hit.transform != null) {
					if (hit.transform.gameObject.name == "PlaneC1") {
						cameraM.enabled = false;
						cameraA.enabled = true;
						cameraB.enabled = false;
                    	//Debug.Log("Hit 1 " + hit.transform.gameObject.name);
					}
					else if (hit.transform.gameObject.name == "PlaneC2") {
						cameraM.enabled = false;
						cameraA.enabled = false;
						cameraB.enabled = true;
                    	//Debug.Log("Hit 2 " + hit.transform.gameObject.name);
					}
//					else if (!cameraM.enabled) {
//						cameraM.enabled = true;
//						cameraA.enabled = false;
//						cameraB.enabled = false;
//						Debug.Log("Hit B " + hit.transform.gameObject.name);
//					}
                }
			}
        }
	}
}
