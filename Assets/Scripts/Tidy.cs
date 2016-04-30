using UnityEngine;
using System.Collections;
using System;

public class Tidy : MonoBehaviour {

    private float initTime;
    void Start ()
    {
        initTime = Time.timeSinceLevelLoad;
    }
	void Update () {

		if(Vector3.Distance(transform.position, new Vector3(0,0,0)) > 10000)
        {
            Destroy(this.gameObject);
        }

        if(Time.timeSinceLevelLoad - initTime > 1000)
        {
            Destroy(this.gameObject);
        }
       
	}

	void OnCollisionEnter(Collision collision){
        try { 
		    GameObject collided = collision.gameObject;
		    collided.GetComponent<Pilot> ().health -= 20;
        }catch(Exception ex)
        {
            Debug.Log("Miss");
        }
        Destroy (this.gameObject);
	}
}
