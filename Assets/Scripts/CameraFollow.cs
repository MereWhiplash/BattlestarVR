using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public GameObject target;
	
	// Update is called once per frame
	void Update () {
        transform.position = target.transform.position + transform.position;
        transform.LookAt(target.transform.position);
	}
}
