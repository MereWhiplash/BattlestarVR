using UnityEngine;
using System.Collections;

public class BoidController : MonoBehaviour {

    public float maxSpeed = 10.0f;
    public float minSpeed = 5.0f;

    public float randomness = 1.0f;

    public int flockSize = 50;

    public GameObject ship;
    public GameObject target;

    public Vector3 centroid;
    public Vector3 velocity;

    private GameObject[] ships;

	// Use this for initialization
	void Start () {
        ships = new GameObject[flockSize];
        for(int i=0;i < flockSize; i++)
        {
            Vector3 position = new Vector3(
                Random.value * GetComponent<Collider>().bounds.size.x,
                Random.value * GetComponent<Collider>().bounds.size.y,
                Random.value * GetComponent<Collider>().bounds.size.z)
                - GetComponent<Collider>().bounds.extents;
            GameObject boid = Instantiate(ship, transform.position, transform.rotation) as GameObject;
            boid.transform.SetParent(transform);//setting
            boid.transform.localPosition = position;
            boid.GetComponent<BoidFlocking>().SetController(gameObject);
            ships[i] = boid;
        }


	}
	
	// Update is called once per frame
	void Update () {
        Vector3 centre = Vector3.zero;
        Vector3 currentVelocity = Vector3.zero;

        foreach(GameObject s in ships)
        {
            centre = centre + s.transform.localPosition;
            currentVelocity = currentVelocity + s.GetComponent<Rigidbody>().velocity;
        }

        centroid = centre / (flockSize);
        velocity = currentVelocity / (flockSize);
	}
}
