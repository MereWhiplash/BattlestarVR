using UnityEngine;
using System.Collections;

public class FlockManager : MonoBehaviour {

    public GameObject shipPrefab;
    public int flockSize;
    public GameObject[] boids;
    public float spawnRadius = 100;

    public GameObject target;
    public Vector3 goalPos;

    public float targetArea;

	// Use this for initialization
	void Start () {
        boids = new GameObject[flockSize];

        for(int i = 0; i < flockSize; i++)
        {
            Vector3 pos = new Vector3(
                Random.insideUnitSphere.x * spawnRadius,
                Random.insideUnitSphere.y * spawnRadius,
                Random.insideUnitSphere.z * spawnRadius);
            GameObject b = Instantiate(shipPrefab, pos, Quaternion.identity) as GameObject;
            b.GetComponent<Flock>().controller = this;
            boids[i] = b;
        }
        //Init Goal
        goalPos = new Vector3(
                Random.insideUnitSphere.x * targetArea,
                Random.insideUnitSphere.y * targetArea,
                Random.insideUnitSphere.z * targetArea) + target.transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        if (Random.Range(0, 10000) < 50)
            goalPos = new Vector3(
                Random.insideUnitSphere.x * targetArea,
                Random.insideUnitSphere.y * targetArea,
                Random.insideUnitSphere.z * targetArea) + target.transform.position;
	}

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(goalPos, 4);
    //}
}
