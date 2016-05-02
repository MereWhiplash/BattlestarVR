using UnityEngine;
using System.Collections;

public class ProjectileMove : MonoBehaviour {


    public float speed = 1000;
    public Vector3 origin;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += Time.deltaTime * speed * transform.forward;
        if(Vector3.Distance(transform.position, origin) > 1000)
        {
            Destroy(this.gameObject);
        }
	}
}
