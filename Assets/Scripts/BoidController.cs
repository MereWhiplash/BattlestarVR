using UnityEngine;
using System.Collections;

public class BoidController : MonoBehaviour {

    public float minVelocity = 60.0f;
    public float maxVelocity = 20.0f;
    public float randomness = 50.0f;
    public int flockSize = 50;

    public GameObject prefab;
    public GameObject target;

    public Vector3 centroid;
    public Vector3 velocity;

    private GameObject[] boids;

	// Use this for initialization
	void Start () {
        boids = new GameObject[flockSize];
        for(int i=0; i < flockSize; i++)
        {
            Vector3 position = new Vector3(
                Random.value * GetComponent<Collider>().bounds.size.x,
                Random.value * GetComponent<Collider>().bounds.size.y,
                Random.value * GetComponent<Collider>().bounds.size.z
            ) - GetComponent<Collider>().bounds.extents;

            GameObject boid = Instantiate(prefab, transform.position, transform.rotation) as GameObject;

            boid.transform.SetParent(transform);//setting
            boid.transform.localPosition = position;
            boid.GetComponent<BoidFlocking>().SetController(gameObject);
            boids[i] = boid;
        }
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 centre = Vector3.zero;
        Vector3 currentVelocity = Vector3.zero;

        foreach(GameObject boid in boids)
        {
            centre = centre + boid.transform.localPosition;
            currentVelocity = currentVelocity + boid.GetComponent<Rigidbody>().velocity;
        }

        centroid = centre / (flockSize);
        velocity = currentVelocity / (flockSize);
	}

    //void OnDrawGizmos()
    //{
    //    Gizmos.DrawSphere(centroid,5);
    //}
}
